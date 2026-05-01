namespace MeteoForecast.Models;

public class WeatherCache : BaseCityCache
{
    public DateTime Date { get; set; }
    public List<HourlyWeather> HourlyData { get; set; } = [];
    public DateTime FetchedAt { get; set; }
}