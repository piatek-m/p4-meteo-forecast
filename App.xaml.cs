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
using System.IO;

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
                        .AddSingleton<MainWindow>(); // Since Shell & DataTemplate are used, only one window needs to register DI.

                })
                .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        host.Start();
        var shell = host.Services.GetRequiredService<ShellViewModel>();
        shell.Initialize();

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var dbPath = Path.Combine(AppContext.BaseDirectory, "meteoforecast.db");
            db.Database.Migrate();

            var cacheService = scope.ServiceProvider.GetRequiredService<IWeatherCacheService>();
            await cacheService.CleanupExpiredAsync();
        }

        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        MainWindow = mainWindow;
        mainWindow.Show();

    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await host.StopAsync();
        host.Dispose();
        base.OnExit(e);
    }
}

