using System.Reflection;
using MeteoForecast.Data.Configurations;
using MeteoForecast.Models;
using Microsoft.EntityFrameworkCore;

namespace MeteoForecast.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<HourlyWeather> HourlyWeathers { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<WeatherAlert> WeatherAlerts { get; set; }
    public DbSet<WeatherCache> WeatherCaches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}