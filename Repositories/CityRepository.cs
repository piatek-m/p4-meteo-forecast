using MeteoForecast.Data;
using MeteoForecast.Models;
using MeteoForecast.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeteoForecast.Repositories;

public class CityRepository : Repository<City>, ICityRepository
{
    public CityRepository(AppDbContext context) : base(context) { }
    public async Task<List<City>> GetFavouritesAsync()
        => await _dbSet
            .Where(c => c.IsFavourite)
            .OrderBy(c => c.Name)
            .ToListAsync();

    public async Task<List<City>> GetRecentAsync(int count)
        => await _context.SearchHistories
            .OrderByDescending(s => s.LastSearchedAt)
            .Take(count)
            .Select(s => s.City)
            .Distinct()
            .ToListAsync();
}