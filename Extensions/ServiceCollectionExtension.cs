using MeteoForecast.Data;
using MeteoForecast.Models;
using MeteoForecast.Models.Settings;
using MeteoForecast.Repositories;
using MeteoForecast.Repositories.Interfaces;
using MeteoForecast.Services;
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
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=meteoforecast.db"));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IWeatherCacheRepository, WeatherCacheRepository>();
        services.AddScoped<IWeatherAlertRepository, WeatherAlertRepository>();
        services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

        return services;
    }

    /* !UNCOMMENT! when I implement the rest of the services

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddHttpClient<IWeatherApiService, OpenMeteoService>();
        services.AddSingleton<ILocationService, NominatimService>();

        services.AddScoped<IWeatherCacheService, WeatherCacheService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<ISettingsService, SettingsService>();

        return services;
    } */

    /*     public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<AlertsViewModel>();
            services.AddTransient<CityWeatherViewModel>();
            services.AddTransient<SearchViewModel>();
            services.AddTransient<SettingsViewModel>();

            return services;
        } */

    // public static IServiceCollection AddViews(this IServiceCollection services)
    // {
    //     services.AddSingleton<MainWindow>();
    //     services.AddSingleton<AlertsView>();
    //     services.AddSingleton<CityWeatherView>();
    //     services.AddSingleton<SearchView>();
    //     services.AddSingleton<SettingsView>();
    // }
}