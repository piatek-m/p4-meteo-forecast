using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using MeteoForecast.Converters.Units;
using MeteoForecast.Models;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Display;
using MeteoForecast.ViewModels.Generics;
namespace MeteoForecast.ViewModels;

public class CityWeatherViewModel : BaseViewModel
{
    private readonly IWeatherCacheService _weatherCacheService;
    private readonly ISettingsService _settingsService;
    private readonly IAlertService _alertService;

    private City? _selectedCity;
    public City? SelectedCity
    {
        get => _selectedCity;
        set
        {
            if (SetProperty(ref _selectedCity, value) && value is not null)
            {
                SelectedDay = DateTime.Today;
                _ = LoadForecastAsync();
            }
        }
    }

    private DateTime _selectedDay = DateTime.Today;
    public DateTime SelectedDay
    {
        get => _selectedDay;
        set
        {
            SetProperty(ref _selectedDay, value);
            OnPropertyChanged(nameof(DayLabel));
        }
    }
    public string DayLabel => (SelectedDay.Date - DateTime.Today).Days switch
    {
        -2 => "przedwczoraj",
        -1 => "wczoraj",
        0 => "dziś",
        1 => "jutro",
        2 => "pojutrze",
        _ => SelectedDay.ToString("ddd d MMM")
    };

    public ObservableCollection<HourlyWeatherDisplay> ForecastDisplay { get; } = [];

    public AsyncRelayCommand NextDayCommand { get; }
    public AsyncRelayCommand PreviousDayCommand { get; }

    public CityWeatherViewModel(
        IWeatherCacheService weatherCacheService,
        ISettingsService settingsService,
        IAlertService alertService)
    {
        _weatherCacheService = weatherCacheService;
        _settingsService = settingsService;
        _alertService = alertService;

        NextDayCommand = new AsyncRelayCommand(async _ => await NextDayAsync());
        PreviousDayCommand = new AsyncRelayCommand(async _ => await PreviousDayAsync());
    }

    public async Task LoadForecastAsync()
    {
        if (SelectedCity is null)
            return;

        var data = await _weatherCacheService.GetWeatherForDayAsync(
            SelectedCity.Id,
            SelectedCity.Latitude,
            SelectedCity.Longitude,
            SelectedDay
        );

        var settings = _settingsService.GetSettings();

        var display = data.Select(h => new HourlyWeatherDisplay
        {
            Time = h.DateTime.ToString("HH:mm"),
            Temperature = UnitConverter.Convert(h.Temperature, UnitType.Temperature, settings),
            FeelsLike = UnitConverter.Convert(h.FeelsLike, UnitType.Temperature, settings),
            Pressure = UnitConverter.Convert(h.Pressure, UnitType.Pressure, settings),
            WindSpeed = UnitConverter.Convert(h.WindSpeed, UnitType.WindSpeed, settings),
            WindDirection = WindConverter.Convert(h.WindDirection, settings.WindDirectionDisplay),
            Humidity = $"{h.Humidity}%",
            Precipitation = $"{h.Precipitation:F1}mm", // Hardcoded, could add inches or whatever option
            Snowfall = $"{h.Snowfall:F1}cm",
            PrecipitationRaw = h.Precipitation, // Because GreaterThanZeroConverter takes double
            SnowfallRaw = h.Snowfall,           // Because GreaterThanZeroConverter takes double
            WeatherCode = h.WeatherCode,
        }).ToList();

        ForecastDisplay.Clear();
        foreach (var item in display)
            ForecastDisplay.Add(item);
        await _alertService.CheckAlertAsync(SelectedCity.Id);
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

    public override void OnNavigatedTo()
        => _ = LoadForecastAsync();
}