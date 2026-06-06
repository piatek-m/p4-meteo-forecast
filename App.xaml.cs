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
        try
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
        catch (Exception ex)
        {
            File.WriteAllText("constructor_app_xaml.log", ex.ToString());
            throw;
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            File.AppendAllText("debug.log", $"Startup begin at {DateTime.Now}\n");

            base.OnStartup(e);
            host.Start();
            File.AppendAllText("debug.log", "Host started\n");

            var shell = host.Services.GetRequiredService<ShellViewModel>();
            shell.Initialize();
            File.AppendAllText("debug.log", "Shell initialized\n");

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var dbPath = Path.Combine(AppContext.BaseDirectory, "meteoforecast.db");
                File.AppendAllText("debug.log", $"DB path: {dbPath}\n");
                db.Database.Migrate();
                File.AppendAllText("debug.log", "Migrate done\n");

                File.AppendAllText("debug.log", "Migrated\n");
                var cacheService = scope.ServiceProvider.GetRequiredService<IWeatherCacheService>();
                await cacheService.CleanupExpiredAsync();
                File.AppendAllText("debug.log", "Cleanup done\n");
            }

            File.AppendAllText("debug.log", "Resolving MainWindow\n");
            try
            {
                var mainWindow = host.Services.GetRequiredService<MainWindow>();
                File.AppendAllText("debug.log", "MainWindow resolved\n");

                MainWindow = mainWindow;
                mainWindow.Show();
                File.AppendAllText("debug.log", "MainWindow shown, YIPEEEE!!!!\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText("startup_app_xaml.log", ex.ToString());
            throw;
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await host.StopAsync();
        host.Dispose();
        base.OnExit(e);
    }
}

