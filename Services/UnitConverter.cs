using System.Windows;
using MeteoForecast.Models;

namespace MeteoForecast.Services;

public static class UnitConverter
{
    // Temperature
    public static double ToCelsius(double fahrenheit)
        => (fahrenheit - 32) * 5 / 9;
    public static double ToFahrenheit(double celsius)
        => celsius * 9 / 5 + 32;

    // Pressure
    public static double ToMmHg(double hPa)
        => hPa * 0.750062;
    public static double ToInHg(double hPa)
        => hPa * 0.029529;

    // Wind speed
    public static double ToKmh(double ms)
        => ms * 3.6;
    public static double ToKnots(double ms)
        => ms * 1.94384;
    public static int ToBeaufort(double ms) => ms switch
    {
        <= 0.2 => 0,
        <= 1.5 => 1,
        <= 3.3 => 2,
        <= 5.4 => 3,
        <= 7.9 => 4,
        <= 10.7 => 5,
        <= 13.8 => 6,
        <= 17.1 => 7,
        <= 20.7 => 8,
        <= 24.4 => 9,
        <= 28.4 => 10,
        <= 32.6 => 11,
        _ => 12
    };

    private static string Convert(double value, UnitType type, AppSettings settings)
        => type switch
        {
            UnitType.Temperature => ConvertTemperature(value, settings.TemperatureUnit),
            UnitType.Pressure => ConvertPressure(value, settings.PressureUnit),
            UnitType.WindSpeed => ConvertWindSpeed(value, settings.WindSpeedUnit),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

    private static string ConvertTemperature(double celsius, TemperatureUnit unit)
        => unit switch
        {
            TemperatureUnit.Celsius => $"{celsius:F1}°C",
            TemperatureUnit.Fahrenheit => $"{ToFahrenheit(celsius):F1}°F",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

    private static string ConvertPressure(double hPa, PressureUnit unit)
        => unit switch
        {
            PressureUnit.hPa => $"{hPa:F1} hPa",
            PressureUnit.mmHg => $"{ToMmHg(hPa):F1} mmHg",
            PressureUnit.inHg => $"{ToInHg(hPa):F2} inHg",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    private static string ConvertWindSpeed(double ms, WindSpeedUnit unit)
        => unit switch
        {
            WindSpeedUnit.MetersPerSecond => $"{ms:F1} m/s",
            WindSpeedUnit.KilometersPerHour => $"{ToKmh(ms):F1} km/h",
            WindSpeedUnit.Knots => $"{ToKnots(ms):F1} kt",
            WindSpeedUnit.Beaufort => $"{ToBeaufort(ms)} Bft",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
}