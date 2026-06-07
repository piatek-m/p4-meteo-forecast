using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Xaml.Schema;
using MeteoForecast.Models;
using MeteoForecast.Repositories.Interfaces;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Generics;
using Windows.Networking.Sockets;

namespace MeteoForecast.ViewModels;

public class SearchViewModel : BaseViewModel
{
    private readonly ILocationService _locationService;
    private readonly ICityRepository _cityRepository;
    private readonly ISearchHistoryRepository _searchHistoryRepository;
    private readonly INavigationService _navigation;

    private readonly double deduplicationTolerance = 0.05;

    public ObservableCollection<FavouriteCityViewModel> Results { get; } = [];
    public ObservableCollection<FavouriteCityViewModel> RecentSearches { get; } = [];

    private string _searchQuery = string.Empty;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
            {
                OnPropertyChanged(nameof(ShowNoResults));
                OnPropertyChanged(nameof(ShowRecents));
            }
        }
    }

    private bool _hasResults;
    public bool HasResults
    {
        get => _hasResults;
        private set
        {
            if (SetProperty(ref _hasResults, value))
            {
                OnPropertyChanged(nameof(ShowNoResults));
                OnPropertyChanged(nameof(ShowRecents));
            }
        }
    }

    public bool ShowNoResults => !HasResults && SearchQuery.Length > 0;
    public bool ShowRecents => SearchQuery.Length == 0 && RecentSearches.Count > 0 && !HasResults;

    public AsyncRelayCommand SearchCommand { get; }
    public AsyncRelayCommand SelectCityCommand { get; }
    public AsyncRelayCommand ToggleFavouriteCommand { get; }
    public AsyncRelayCommand UseCurrentLocationCommand { get; }

    public SearchViewModel(
        ILocationService locationService,
        ICityRepository cityRepository,
        ISearchHistoryRepository searchHistoryRepository,
        INavigationService navigation
    )
    {
        _locationService = locationService;
        _cityRepository = cityRepository;
        _searchHistoryRepository = searchHistoryRepository;
        _navigation = navigation;

        SearchCommand = new AsyncRelayCommand(
            async _ => await SearchAsync(_searchQuery)
        );
        SelectCityCommand = new AsyncRelayCommand(
            async param => await SelectCityAsync(param as City)
        );
        ToggleFavouriteCommand = new AsyncRelayCommand(
            async param => await ToggleFavouriteAsync(param as FavouriteCityViewModel)
        );
        UseCurrentLocationCommand = new AsyncRelayCommand(
            async _ => await SearchByLocationAsync()
        );
    }

    private bool _shouldFocusSearch;
    public bool ShouldFocusSearch
    {
        get => _shouldFocusSearch;
        set => SetProperty(ref _shouldFocusSearch, value);
    }
    public override void OnNavigatedTo()
    {
        Results.Clear();
        HasResults = false;
        ShouldFocusSearch = true;
        _ = LoadRecentAsync();
    }

    private async Task LoadRecentAsync()
    {
        var recent = await _searchHistoryRepository.GetRecentAsync(10);
        var savedCities = await _cityRepository.GetAllAsync();

        RecentSearches.Clear();
        foreach (var entry in recent)
        {
            var existing = savedCities.FirstOrDefault(c => c.Id == entry.CityId);
            if (existing is not null)
                RecentSearches.Add(new FavouriteCityViewModel(existing));
        }
        OnPropertyChanged(nameof(ShowRecents));
    }

    private async Task SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            Results.Clear();
            HasResults = false;
            return;
        }

        var cities = await _locationService.SearchCitiesAsync(query);
        cities = cities
                .GroupBy(c => (
                    Math.Round(c.Latitude, 0),
                    Math.Round(c.Longitude, 0))
                )
                .Select(g => g.First())
                .ToList();


        var savedCities = await _cityRepository.GetAllAsync();

        Results.Clear();
        foreach (var city in cities)
        {
            var existing = savedCities.FirstOrDefault(c =>
                Math.Abs(c.Latitude - city.Latitude) < deduplicationTolerance &&
                Math.Abs(c.Longitude - city.Longitude) < deduplicationTolerance);

            var vm = new FavouriteCityViewModel(existing ?? city);
            Results.Add(vm);
        }
        HasResults = Results.Count > 0;
    }

    private async Task SearchByLocationAsync()
    {
        try
        {
            var (lat, lon) = await _locationService.GetCurrentLocationAsync();
            var city = await _locationService.GetCityByLocationAsync(lat, lon);
            if (city is null) return;

            Results.Clear();
            Results.Add(new FavouriteCityViewModel(city));
            HasResults = true;
        }
        catch (InvalidOperationException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    private async Task SelectCityAsync(City? city)
    {
        if (city is null) return;

        var existing = (await _cityRepository.GetAllAsync())
            .FirstOrDefault(c =>
                Math.Abs(c.Latitude - city.Latitude) < deduplicationTolerance &&
                Math.Abs(c.Longitude - city.Longitude) < deduplicationTolerance);

        if (existing is null)
        {
            await _cityRepository.AddAsync(city);
            await _cityRepository.SaveChangesAsync();
            existing = city;
        }

        await _searchHistoryRepository.AddAsync(new SearchHistory
        {
            CityId = existing.Id,
            LastSearchedAt = DateTime.Now
        });
        await _searchHistoryRepository.SaveChangesAsync();

        _navigation.NavigateTo<MainViewModel>(vm => vm.SelectedCity = existing);
    }

    private async Task ToggleFavouriteAsync(FavouriteCityViewModel? vm)
    {
        if (vm is null) return;

        var city = vm.City;
        vm.IsFavourite = !vm.IsFavourite;

        var existing = (await _cityRepository.GetAllAsync())
            .FirstOrDefault(c =>
                Math.Abs(c.Latitude - city.Latitude) < deduplicationTolerance &&
                Math.Abs(c.Longitude - city.Longitude) < deduplicationTolerance);

        if (existing is not null)
        {
            existing.IsFavourite = city.IsFavourite;
            await _cityRepository.UpdateAsync(existing);
            await _cityRepository.SaveChangesAsync();
        }

        vm.City.IsFavourite = city.IsFavourite;
        OnPropertyChanged(nameof(Results));
    }

}