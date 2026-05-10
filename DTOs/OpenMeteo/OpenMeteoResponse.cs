using System.Text.Json.Serialization;

namespace MeteoForecast.OpenMeteo.DTOs;

public class OpenMeteoResponse
{
    [JsonPropertyName("hourly")]
    public HourlyData? Hourly { get; set; }
}