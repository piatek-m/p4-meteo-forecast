using System.Text.Json.Serialization;

namespace MeteoForecast.DTOs.Nominatim;

public class NominatimResult
{
    [JsonPropertyName("lat")]
    public string Lat { get; set; } = "";

    [JsonPropertyName("lon")]
    public string Lon { get; set; } = "";

    [JsonPropertyName("addresss")]
    public NominatimAddress? Addresss { get; set; }
}