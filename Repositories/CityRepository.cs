using MeteoForecast.Data;
using MeteoForecast.Models;
using MeteoForecast.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeteoForecast.Repositories;

public class CityRepository(AppDbContext context) : Repository<City>(context), ICityRepository
{
    public async Task<List<City>> GetFavouritesAsync()
        => await _dbSet
            .Where(c => c.IsFavourite)
            .OrderBy(c => c.Name)
            .ToListAsync();

    // Probably redundant as SearchHistory already has GetRecentAsync but keep for now
    public async Task<List<City>> GetRecentAsync(int count)
        => await _context.SearchHistories
            .OrderByDescending(s => s.LastSearchedAt)
            .Take(count)
            .Select(s => s.City)
            .Distinct()
            .ToListAsync();
}