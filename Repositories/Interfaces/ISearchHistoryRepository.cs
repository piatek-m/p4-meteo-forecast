using MeteoForecast.Models;

namespace MeteoForecast.Repositories.Interfaces;

public interface ISearchHistoryRepository : IRepository<SearchHistory>
{
    Task<List<SearchHistory>> GetRecentAsync(int count);
    Task ClearAsync();
}