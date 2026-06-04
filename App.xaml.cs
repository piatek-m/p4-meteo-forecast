using System.Configuration;
using System.Data;
using System.Windows;
using MeteoForecast.Data;
using MeteoForecast.Repositories;
using MeteoForecast.Models;
using MeteoForecast.Services;
using MeteoForecast.ViewModels;
using MeteoForecast.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MeteoForecast.Extensions;
using MeteoForecast.Repositories.Interfaces;
using MeteoForecast.Services.Interfaces;

namespace MeteoForecast;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost host;

    public App()
    {
        host = Host.CreateDefaultBuilder()

            .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true);
                })

                .ConfigureServices((context, services) =>
                {
                    services
                        .AddConfiguration(context.Configuration)
                        .AddDatabase()
                        .AddRepositories()
                        .AddAppServices()
                        .AddViewModels()
                        .AddViews();

                })
                .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await host.StartAsync();

        // Clear old weather cache oon startup
        var cacheService = host.Services.GetRequiredService<IWeatherCacheService>();
        await cacheService.CleanupExpiredAsync();

        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await host.StopAsync();
        host.Dispose();
        base.OnExit(e);
    }
}

