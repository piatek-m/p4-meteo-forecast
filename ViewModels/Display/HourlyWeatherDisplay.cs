namespace MeteoForecast.ViewModels.Display;

public class HourlyWeatherDisplay
{
    public required string Time { get; init; }
    public required string Temperature { get; init; }
    public required string FeelsLike { get; init; }
    public required string Pressure { get; init; }
    public required string WindSpeed { get; init; }
    public required string WindDirection { get; init; }
    public required string Humidity { get; init; }
    public required string Precipitation { get; init; }
    public required string Snowfall { get; init; }
    public double PrecipitationRaw { get; init; }
    public double SnowfallRaw { get; init; }
    public int WeatherCode { get; init; }
}