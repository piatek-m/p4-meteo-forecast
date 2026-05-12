using System.Drawing;
using System.Net.Http;
using MeteoForecast.DTOs.Nominatim;
using MeteoForecast.Models;
using MeteoForecast.Models.Settings;
using MeteoForecast.Services.Interfaces;
using Microsoft.Extensions.Options;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

namespace MeteoForecast.Services.APIs;

public class NominatimService : BaseHttpApiService, ILocationService
{
    public NominatimService(IOptions<NominatimSettings> nominatimSettings) : base(CreateHttpClient(nominatimSettings.Value.UserAgent), "https://nominatim.openstreetmap.org/search") { }

    private static HttpClient CreateHttpClient(string userAgent)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        return client;
    }

    public async Task<List<City>> SearchCitiesAsync(string query)
    {
        var url =
            $"{ApiUrl}?q={Uri.EscapeDataString(query)}" +
            $"&format=json&addressdetails=1&limit=10";

        var results = await GetAsync<List<NominatimResult>>(url) ?? [];

        return [.. results.Select(MapResponseToCity)];
    }

    public async Task<(double Lat, double Lon)> GetCurrentLocationAsync()
    {
        var geolocator = new Geolocator
        {
            DesiredAccuracy = PositionAccuracy.Default
        };

        var position = await geolocator.GetGeopositionAsync();
        var coord = position.Coordinate.Point.Position;

        return (coord.Latitude, coord.Longitude);
    }

    private static City MapResponseToCity(NominatimResult result) => new()
    {
        Name = result.Address?.ResolveName() ?? "Unknown",
        Country = result.Address?.Country ?? "Unknown",
        Latitude = double.Parse(result.Lat, System.Globalization.CultureInfo.InvariantCulture),
        Longitude = double.Parse(result.Lon, System.Globalization.CultureInfo.InvariantCulture),
        AddedAt = DateTime.Now
    };

}