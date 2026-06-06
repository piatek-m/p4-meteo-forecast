using System.Collections.ObjectModel;
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
    private readonly ISearchHistoryRepository _searchHistoryRepository;
    private readonly IWeatherCacheService _weatherCacheService;
    private readonly INavigationService _navigation;

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
            // _navigation.NavigateTo<CityWeatherViewModel>(vm => vm.SelectedCity = value);
        }
    }

    public AsyncRelayCommand ToggleFavouritesCommand { get; }

    public RelayCommand SelectCityCommand { get; }

    public MainViewModel(
        ICityRepository cityRepository,
        ISearchHistoryRepository searchHistoryRepository,
        IWeatherCacheService weatherCacheService,
        INavigationService navigation,
        CityWeatherViewModel cityWeather
    )
    {
        _cityRepository = cityRepository;
        _searchHistoryRepository = searchHistoryRepository;
        _weatherCacheService = weatherCacheService;
        _navigation = navigation;

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

    public override void OnNavigatedTo()
    {
        System.Diagnostics.Debug.WriteLine("MainViewModel.OnNavigatedTo called");
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var favourites = await _cityRepository.GetFavouritesAsync();
        Favourites.Clear();
        foreach (var city in favourites)
        {
            var vm = new FavouriteCityViewModel(city);
            Favourites.Add(vm);
            _ = LoadTemperatureAsync(vm);
        }

        var recent = await _searchHistoryRepository.GetRecentAsync(10);
        RecentSearches.Clear();
        foreach (var entry in recent)
            RecentSearches.Add(entry);
    }

    private async Task LoadTemperatureAsync(FavouriteCityViewModel vm)
    {
        var data = await _weatherCacheService.GetWeatherForDayAsync(
            vm.City.Id,
            vm.City.Latitude,
            vm.City.Longitude,
            DateTime.Today
        );

        var now = DateTime.Now;
        var closest = data
            .Where(h => h.DateTime <= now)
            .MaxBy(h => h.DateTime);

        App.Current.Dispatcher.Invoke(() =>
        {
            vm.Temperature = closest?.Temperature;
            vm.WeatherCode = closest?.WeatherCode;
        });

        // RunOnUI(() => vm.Temperature = closest?.Temperature);
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