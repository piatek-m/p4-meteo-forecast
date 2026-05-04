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
                    config.AddJsonFile("appsettings.json,",
                    optional: false,
                    reloadOnChange: true);
                })

                .ConfigureServices((context, services) =>
                {
                    services.Configure<AppSettings>(
                        context.Configuration.GetSection("AppSettings")
                    );

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite("Data Source=meteoforecast.db"));

                    services
                        .AddRepositories()
                        .AddAppServices()
                        .AddViewModels();

                    services.AddSingleton<MainWindow>();
                    // services.AddSingleton<AlertsView>();
                    // services.AddSingleton<CityWeatherView>();
                    // services.AddSingleton<SearchView>();
                    // services.AddSingleton<SettingsView>();

                })
                .Build();
    }
    protected override async void OnStartup(StartupEventArgs e)
    {
        await host.StartAsync();

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

