using System.Drawing;
using System.Globalization;
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
    private readonly Dictionary<string, List<City>> _cache = [];
    private DateTime lastRequestTime = DateTime.MinValue;
    private static readonly TimeSpan MinInterval = TimeSpan.FromSeconds(1);

    public NominatimService(IOptions<NominatimSettings> nominatimSettings) : base(CreateHttpClient(nominatimSettings.Value.UserAgent), "https://nominatim.openstreetmap.org/search") { }

    private static HttpClient CreateHttpClient(string userAgent)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        return client;
    }

    public async Task<List<City>> SearchCitiesAsync(string query)
    {
        var key = query.Trim().ToLower();
        if (_cache.TryGetValue(key, out var cached))
            return cached;

        await EnforceRateLimitAsync();

        var url =
            $"{ApiUrl}?q={Uri.EscapeDataString(query)}" +
            $"&format=json&addressdetails=1&limit=10" +
            $"&namedetails=0&extratags=0" +
            $"&accept-language=pl" +
            $"&featuretype=city,town,village";

        var results = await GetAsync<List<NominatimResult>>(url) ?? [];
        var filtered = results.Where(r => r.AddressType is "city" or "town" or "village" or "hamlet" or "suburb");
        var cities = filtered.Select(MapResponseToCity).ToList();

        _cache[key] = cities;
        return cities;
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

    public async Task<City?> GetCityByLocationAsync(double lat, double lon)
    {
        await EnforceRateLimitAsync();
        var url =
            $"https://nominatim.openstreetmap.org/reverse" +
            $"?lat={lat.ToString(CultureInfo.InvariantCulture)}" +
            $"&lon={lon.ToString(CultureInfo.InvariantCulture)}" +
            $"&format=json&addressdetails=1" +
            $"&accept-language=pl";

        var result = await GetAsync<NominatimResult>(url);
        if (result is null) return null;
        return MapResponseToCity(result);
    }

    private static City MapResponseToCity(NominatimResult result) => new()
    {
        Name = result.Address?.ResolveName() is { Length: > 0 } name && name != "Unkown"
            ? name
            : result.Name is { Length: > 0 } ? result.Name : "Unkown",
        Country = result.Address?.ResolveSubtitle() is { Length: > 0 } subtitle
            ? $"{subtitle}, {result.Address?.Country}"
            : result.Address?.Country ?? "Unknown",
        Latitude = double.Parse(result.Lat, System.Globalization.CultureInfo.InvariantCulture),
        Longitude = double.Parse(result.Lon, System.Globalization.CultureInfo.InvariantCulture),
        AddedAt = DateTime.Now
    };

    private async Task EnforceRateLimitAsync()
    {
        var elapsed = DateTime.Now - lastRequestTime;
        if (elapsed < MinInterval)
            await Task.Delay(MinInterval - elapsed);
        lastRequestTime = DateTime.Now;
    }
}