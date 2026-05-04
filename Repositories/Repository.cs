using MeteoForecast.Data;
using MeteoForecast.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeteoForecast.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
        => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);
    public async Task UpdateAsync(T entity)
        => _dbSet.Update(entity);
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
            _dbSet.Remove(entity);
    }
}