using System.Net.Http;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.Models;
using System.Net.Http.Json;
using MeteoForecast.DTOs.OpenMeteo;

namespace MeteoForecast.Services;

public class OpenMeteoService(HttpClient httpClient) : IWeatherApiService
{
    private readonly HttpClient _httpClient = httpClient;

    private const string OpenMeteoUrl = "https://api.open-meteo.com/v1/forecast";
    private const string HourlyFields =
        "temperature_2m,apparent_temperature,precipitation,snowfall," +
        "surface_pressure,wind_speed_10m,wind_direction_10m," +
        "relative_humidity_2m,weather_code";

    public async Task<List<HourlyWeather>> FetchHourlyAsync(double lat, double lon, DateTime day)
    {
        var date = day.ToString("yyyy-MM-dd");
        var url =
            $"{OpenMeteoUrl}?latitude={lat}&longitude={lon}" +
            $"&hourly={HourlyFields}" +
            $"&start_date={date}&end_date={date}";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url)
            ?? throw new InvalidOperationException("OpenMeteo returned empty response.");

        return MapToHourlyWeather(response);
    }

    private static List<HourlyWeather> MapToHourlyWeather(OpenMeteoResponse response)
    {
        var hourly = response.Hourly
            ?? throw new InvalidOperationException("Response contains no time data");

        return hourly.Time.Select((time, i) => new HourlyWeather
        {
            DateTime = DateTime.Parse(time),
            Temperature = hourly.Temperature[i],
            FeelsLike = hourly.FeelsLike[i],
            Precipitation = hourly.Precipitation[i],
            Snowfall = hourly.Snowfall[i],
            Pressure = hourly.Pressure[i],
            WindSpeed = hourly.WindSpeed[i],
            WindDirection = hourly.WindDirection[i],
            Humidity = hourly.Humidity[i],
            WeatherCode = hourly.WeatherCode[i],
        }).ToList();
    }
}