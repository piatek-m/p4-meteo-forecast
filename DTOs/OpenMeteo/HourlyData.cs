using System.Text.Json.Serialization;

namespace MeteoForecast.DTOs.OpenMeteo;

public class HourlyData
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = [];
    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature { get; set; } = [];

    [JsonPropertyName("apparent_temperature")]
    public List<double> FeelsLike { get; set; } = [];

    [JsonPropertyName("precipitation")]
    public List<double> Precipitation { get; set; } = [];

    [JsonPropertyName("snowfall")]
    public List<double> Snowfall { get; set; } = [];

    [JsonPropertyName("surface_pressure")]
    public List<double> Pressure { get; set; } = [];

    [JsonPropertyName("wind_speed_10m")]
    public List<double> WindSpeed { get; set; } = [];

    [JsonPropertyName("wind_direction_10m")]
    public List<int> WindDirection { get; set; } = [];

    [JsonPropertyName("relative_humidity_2m")]
    public List<int> Humidity { get; set; } = [];

    [JsonPropertyName("weather_code")]
    public List<string> WeatherCode { get; set; } = [];
}