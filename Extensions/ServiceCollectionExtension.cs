using System.IO;
using MeteoForecast.Data;
using MeteoForecast.Models;
using MeteoForecast.Models.Settings;
using MeteoForecast.Repositories;
using MeteoForecast.Repositories.Interfaces;
using MeteoForecast.Services;
using MeteoForecast.Services.APIs;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels;
using MeteoForecast.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeteoForecast.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(
            configuration.GetSection("AppSettings"));
        services.Configure<NominatimSettings>(
            configuration.GetSection("NominatimSettings"));

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "meteoforecast.db");
        File.AppendAllText("db_path.log", dbPath + "\n");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IWeatherCacheRepository, WeatherCacheRepository>();
        services.AddTransient<IWeatherAlertRepository, WeatherAlertRepository>();
        services.AddTransient<ISearchHistoryRepository, SearchHistoryRepository>();

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddHttpClient<IWeatherApiService, OpenMeteoService>();
        services.AddSingleton<ILocationService, NominatimService>();

        services.AddTransient<IWeatherCacheService, WeatherCacheService>();
        services.AddTransient<IAlertService, AlertService>();
        services.AddTransient<ISettingsService, SettingsService>();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton(sp => (INavigationService)sp.GetRequiredService<ShellViewModel>());
        services.AddSingleton<MainViewModel>();
        // services.AddSingleton<AlertsViewModel>();
        services.AddSingleton<CityWeatherViewModel>();
        services.AddSingleton<SearchViewModel>();
        services.AddSingleton<SettingsViewModel>();

        return services;
    }
}