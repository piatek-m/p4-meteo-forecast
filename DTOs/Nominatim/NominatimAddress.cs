using System.Text.Json.Serialization;

namespace MeteoForecast.DTOs.Nominatim;

public class NominatimAddress
{
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("town")]
    public string? Town { get; set; }

    [JsonPropertyName("village")]
    public string? Village { get; set; }

    [JsonPropertyName("municipality")]
    public string? Municipality { get; set; }

    [JsonPropertyName("county")]
    public string? County { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    public string ResolveName() => City ?? Town ?? Village ?? "Unkown";

    public string ResolveSubtitle()
    {
        var parts = new[] { Municipality, County, State }
            .Where(p => !string.IsNullOrEmpty(p));
        return string.Join(", ", parts);
    }
}