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

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    public string ResolveName() => City ?? Town ?? Village ?? "Unkown";
}