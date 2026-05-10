
namespace MeteoForecast.Models;

public class HourlyWeather
{
    public int Id { get; set; }
    public int WeatherCacheId { get; set; }
    public DateTime DateTime { get; set; }
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public double Precipitation { get; set; } // mm
    public double Snowfall { get; set; } // mm
    public double Pressure { get; set; } // hPa
    public double WindSpeed { get; set; } // m/s
    public double WindDirection { get; set; } // deg. (0-360)
    public int Humidity { get; set; }
    public required string WeatherCode { get; set; }
}
