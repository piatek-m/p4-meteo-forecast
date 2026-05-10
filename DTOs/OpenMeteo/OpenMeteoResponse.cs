using System.Text.Json.Serialization;

namespace MeteoForecast.DTOs.OpenMeteo;

public class OpenMeteoResponse
{
    [JsonPropertyName("hourly")]
    public HourlyData? Hourly { get; set; }
}