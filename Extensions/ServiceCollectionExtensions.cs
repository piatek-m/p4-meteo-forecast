using MeteoForecast.Repositories;
using MeteoForecast.Repositories.Interfaces;
using MeteoForecast.Services;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels;
using MeteoForecast.Views;


using Microsoft.Extensions.DependencyInjection;

namespace MeteoForecast.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherCacheService, WeatherCacheService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<ISettingsService, SettingsService>();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<AlertsViewModel>();
        services.AddTransient<CityWeatherViewModel>();
        services.AddTransient<SearchViewModel>();
        services.AddTransient<SettingsViewModel>();

        return services;
    }

}