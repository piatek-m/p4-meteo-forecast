using System.Reflection;
using MeteoForecast.Data.Configurations;
using MeteoForecast.Models;
using Microsoft.EntityFrameworkCore;

namespace MeteoForecast.Data;

internal class AppDbContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<HourlyWeather> HourlyWeathers { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<WeatherAlert> WeatherAlerts { get; set; }
    public DbSet<WeatherCache> WeatherCaches { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}