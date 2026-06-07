using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reflection;
using System.Security.Permissions;
using MeteoForecast.Models;
using MeteoForecast.Repositories.Interfaces;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Generics;

namespace MeteoForecast.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ICityRepository _cityRepository;
    private readonly IWeatherApiService _weatherApiService;

    public CityWeatherViewModel CityWeather { get; }

    public ObservableCollection<FavouriteCityViewModel> Favourites { get; } = [];
    public ObservableCollection<SearchHistory> RecentSearches { get; } = [];

    private City? _selectedCity;
    public City? SelectedCity
    {
        get => _selectedCity;
        set
        {
            if (SetProperty(ref _selectedCity, value) && value is not null)
                CityWeather.SelectedCity = value;
        }
    }

    public AsyncRelayCommand ToggleFavouritesCommand { get; }

    public RelayCommand SelectCityCommand { get; }

    public MainViewModel(
        ICityRepository cityRepository,
        CityWeatherViewModel cityWeather,
        IWeatherApiService weatherApiService
    )
    {
        _cityRepository = cityRepository;
        _weatherApiService = weatherApiService;

        CityWeather = cityWeather;

        ToggleFavouritesCommand = new AsyncRelayCommand(
            async param => await ToggleFavouritesAsync(param as City)
        );

        SelectCityCommand = new RelayCommand(
            param =>
        {
            if (param is City city)
                SelectedCity = city;
        });
    }

    public void OnNavigatedToFrom(BaseViewModel? previous)
    {
        OnNavigatedTo();
        if (previous is SettingsViewModel && CityWeather.SelectedCity is not null)
            _ = CityWeather.LoadForecastAsync();
    }

    public override void OnNavigatedTo()
    {
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var favourites = await _cityRepository.GetFavouritesAsync();
        Favourites.Clear();

        var vms = favourites.Select(city => new FavouriteCityViewModel(city)).ToList();
        foreach (var vm in vms)
            Favourites.Add(vm);

        if (vms.Count > 0)
            await LoadTemperaturesBatchAsync(vms);
    }

    private async Task LoadTemperaturesBatchAsync(List<FavouriteCityViewModel> vms)
    {
        var locations = vms
            .Select(vm => (vm.City.Latitude, vm.City.Longitude))
            .ToList();

        var results = await _weatherApiService.FetchHourlyBatchAsync(locations, DateTime.Today);

        var now = DateTime.Now;
        for (int i = 0; i < vms.Count; i++)
        {
            var closest = results[i]
                .Where(h => h.DateTime <= now)
                .MaxBy(h => h.DateTime);
            vms[i].Temperature = closest?.Temperature;
            vms[i].WeatherCode = closest?.WeatherCode;
        }
    }

    private async Task ToggleFavouritesAsync(City? city)
    {
        if (city is null) return;

        city.IsFavourite = !city.IsFavourite;
        await _cityRepository.UpdateAsync(city);
        await _cityRepository.SaveChangesAsync();

        await LoadDataAsync();
    }
}