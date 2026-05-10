using MeteoForecast.Models;

namespace MeteoForecast.Services.Interfaces;

public interface ILocationService
{
    Task<List<City>> SearchCitiesAsync(string query);
    Task<(double Lat, double Lon)> GetCurrentLocationAsync();
}