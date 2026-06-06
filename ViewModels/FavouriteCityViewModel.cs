using System.Configuration;
using MeteoForecast.Models;
using MeteoForecast.ViewModels.Generics;

namespace MeteoForecast.ViewModels;

public class FavouriteCityViewModel(City city) : BaseViewModel
{
    public City City { get; } = city;
    private bool _isFavourite = city.IsFavourite;
    public bool IsFavourite
    {
        get
        {
            return _isFavourite;
        }
        set
        {
            if (SetProperty(ref _isFavourite, value))
                City.IsFavourite = value;
        }
    }

    private double? _temperature;
    public double? Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }

    private int? _weatherCode;
    public int? WeatherCode
    {
        get => _weatherCode;
        set => SetProperty(ref _weatherCode, value);
    }
}