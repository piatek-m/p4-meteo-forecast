using MeteoForecast.Models;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Generics;

namespace MeteoForecast.ViewModels;

public class SettingsViewModel(ISettingsService settingsService) : BaseViewModel
{
    private readonly ISettingsService _settingsService = settingsService;
    private AppSettings _settings = new();

    public IEnumerable<TemperatureUnit> TemperatureUnits
        => Enum.GetValues<TemperatureUnit>();

    private TemperatureUnit _temperatureUnit;
    public TemperatureUnit TemperatureUnit
    {
        get => _temperatureUnit;
        set
        {
            if (SetProperty(ref _temperatureUnit, value))
                _settings.TemperatureUnit = value;
        }
    }

    public IEnumerable<PressureUnit> PressureUnits
        => Enum.GetValues<PressureUnit>();

    private PressureUnit _pressureUnit;
    public PressureUnit PressureUnit
    {
        get => _pressureUnit;
        set
        {
            if (SetProperty(ref _pressureUnit, value))
                _settings.PressureUnit = value;
        }
    }

    public IEnumerable<WindSpeedUnit> WindSpeedUnits
        => Enum.GetValues<WindSpeedUnit>();

    private WindSpeedUnit _windSpeedUnit;
    public WindSpeedUnit WindSpeedUnit
    {
        get => _windSpeedUnit;
        set
        {
            if (SetProperty(ref _windSpeedUnit, value))
                _settings.WindSpeedUnit = value;
        }
    }

    public IEnumerable<WindDirectionDisplay> WindDirectionDisplays
        => Enum.GetValues<WindDirectionDisplay>();

    private WindDirectionDisplay _windDirectionDisplay;
    public WindDirectionDisplay WindDirectionDisplay
    {
        get => _windDirectionDisplay;
        set
        {
            if (SetProperty(ref _windDirectionDisplay, value))
                _settings.WindDirectionDisplay = value;
        }
    }

    private int _cacheIntervalMinutes;
    public int CacheIntervalMinutes
    {
        get => _cacheIntervalMinutes;
        set
        {
            if (SetProperty(ref _cacheIntervalMinutes, value))
                _settings.CacheIntervalMinutes = value;
        }
    }

    public override void OnNavigatedTo()
    {
        _settings = _settingsService.GetSettings();
        TemperatureUnit = _settings.TemperatureUnit;
        PressureUnit = _settings.PressureUnit;
        WindSpeedUnit = _settings.WindSpeedUnit;
        WindDirectionDisplay = _settings.WindDirectionDisplay;
        CacheIntervalMinutes = _settings.CacheIntervalMinutes;
    }

    public async Task SaveAsync()
        => await _settingsService.SaveSettingsAsync(_settings);
}