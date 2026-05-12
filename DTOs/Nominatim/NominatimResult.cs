using System.Text.Json.Serialization;

namespace MeteoForecast.DTOs.Nominatim;

public class NominatimResult
{
    [JsonPropertyName("lat")]
    public string Lat { get; set; } = "";

    [JsonPropertyName("lon")]
    public string Lon { get; set; } = "";

    [JsonPropertyName("address")]
    public NominatimAddress? Address { get; set; }
}