using System.Net.Http;
using System.Net.Http.Json;

namespace MeteoForecast.Services.APIs;

public abstract class BaseHttpApiService(HttpClient httpClient, string apiUrl)
{
    protected readonly HttpClient _httpClient = httpClient;

    protected readonly string ApiUrl = apiUrl;

    protected async Task<T> GetAsync<T>(string url)
    {
        var result = await _httpClient.GetFromJsonAsync<T>(url)
            ?? throw new InvalidOperationException($"Empty response from: {url}");
        return result;
    }
}