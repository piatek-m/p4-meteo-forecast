using System.Collections.ObjectModel;
using System.Windows.Input;
using MeteoForecast.Models;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Generics;
namespace MeteoForecast.ViewModels;

public class CityWeatherViewModel : BaseViewModel
{
    private readonly IWeatherCacheService _weatherCacheService;

    private City? _selectedCity;
    public City? SelectedCity
    {
        get => _selectedCity;
        set => SetProperty(ref _selectedCity, value);
    }

    private DateTime _selectedDay = DateTime.Today;
    public DateTime SelectedDay
    {
        get => _selectedDay;
        set => SetProperty(ref _selectedDay, value);
    }
    public ObservableCollection<HourlyWeather> Forecast { get; } = [];

    public ICommand NextDayCommand { get; }
    public ICommand PreviousDayCommand { get; }

    public CityWeatherViewModel(IWeatherCacheService weatherCacheService)
    {
        _weatherCacheService = weatherCacheService;

        NextDayCommand = new AsyncRelayCommand(async _ => await NextDayAsync());
        PreviousDayCommand = new AsyncRelayCommand(async _ => await PreviousDayAsync());
    }

    private async Task LoadForecastAsync()
    {
        if (SelectedCity is null)
            return;

        var data = await _weatherCacheService.GetWeatherForDayAsync(
            SelectedCity.Id,
            SelectedCity.Latitude,
            SelectedCity.Longitude,
            SelectedDay
        );
        Forecast.Clear();
        foreach (var item in data)
            Forecast.Add(item);
    }

    private async Task NextDayAsync()
    {
        if (SelectedDay >= DateTime.Today.AddDays(2)) return;
        SelectedDay = SelectedDay.AddDays(1);
        await LoadForecastAsync();
    }
    private async Task PreviousDayAsync()
    {
        if (SelectedDay <= DateTime.Today.AddDays(-2)) return;
        SelectedDay = SelectedDay.AddDays(-1);
        await LoadForecastAsync();
    }

}