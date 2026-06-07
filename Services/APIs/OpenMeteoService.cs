using System.Net.Http;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.Models;
using System.Net.Http.Json;
using MeteoForecast.DTOs.OpenMeteo;
using System.IO;
using System.Text.Json;

namespace MeteoForecast.Services.APIs;

public class OpenMeteoService(HttpClient httpClient) : BaseHttpApiService(httpClient, "https://api.open-meteo.com/v1/forecast"), IWeatherApiService
{
    private const string HourlyFields =
        "temperature_2m,apparent_temperature,precipitation,snowfall," +
        "surface_pressure,wind_speed_10m,wind_direction_10m," +
        "relative_humidity_2m,weather_code";

    public async Task<List<HourlyWeather>> FetchHourlyAsync(double lat, double lon, DateTime day)
    {
        System.Diagnostics.Debug.WriteLine($"API call for {day:yyyy-MM-dd}");

        var date = day.ToString("yyyy-MM-dd");
        var url =
            $"{ApiUrl}?latitude={lat}&longitude={lon}" +
            $"&hourly={HourlyFields}" +
            $"&start_date={date}&end_date={date}";

        var response = await GetAsync<OpenMeteoResponse>(url);
        return MapResponseToWeather(response);
    }

    public async Task<List<List<HourlyWeather>>> FetchHourlyBatchAsync(IList<(double Lat, double Lon)> locations, DateTime day)
    {
        System.Diagnostics.Debug.WriteLine($"API call for {day:yyyy-MM-dd}, using FetchBatch");

        if (locations.Count == 1)
        {
            var single = await FetchHourlyAsync(
                locations[0].Lat, locations[0].Lon, day);
            return [single];
        }
        var date = day.ToString("yyyy-MM-dd");
        var lats = string.Join(",", locations.Select(l => l.Lat));
        var lons = string.Join(",", locations.Select(l => l.Lon));

        var url =
            $"{ApiUrl}?latitude={lats}&longitude={lons}" +
            $"&hourly={HourlyFields}" +
            $"&start_date={date}&end_date={date}";

        var responses = await GetAsync<List<OpenMeteoResponse>>(url);

        return [.. responses.Select(MapResponseToWeather)];
    }

    private static List<HourlyWeather> MapResponseToWeather(OpenMeteoResponse response)
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