using MeteoForecast.Models;

namespace MeteoForecast.Repositories.Interfaces;

public interface IWeatherCacheRepository : IRepository<WeatherCache>
{
    Task<WeatherCache?> GetByCityAndDateAsync(int cityId, DateTime date);
    Task DeleteExpiredAsync(int cacheIntervalMinutes);
}