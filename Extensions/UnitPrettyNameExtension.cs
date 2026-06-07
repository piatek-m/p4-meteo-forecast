using MeteoForecast.Models;

namespace MeteoForecast.Extensions;

public static class UnitPrettyNameExtension
{
    public static string DisplayName(this WindSpeedUnit unit) => unit switch
    {
        WindSpeedUnit.MetersPerSecond => "m/s",
        WindSpeedUnit.KilometersPerHour => "km/h",
        WindSpeedUnit.Knots => "węzły",
        WindSpeedUnit.Beaufort => "skala Beauforta",
        _ => unit.ToString()
    };

    public static string DisplayName(this TemperatureUnit unit) => unit switch
    {
        TemperatureUnit.Celsius => "stopnie Celsjusza (°C)",
        TemperatureUnit.Fahrenheit => "stopnie Fahrenheita (°F)",
        _ => unit.ToString()
    };

    public static string DisplayName(this PressureUnit unit) => unit switch
    {
        PressureUnit.hPa => "hPa",
        PressureUnit.mmHg => "mmHg",
        PressureUnit.inHg => "inHg",
        _ => unit.ToString()
    };

    public static string DisplayName(this WindDirectionDisplay unit) => unit switch
    {
        WindDirectionDisplay.Cardinal => "kierunki geograficzne",
        WindDirectionDisplay.Arrow => "strzałki",
        _ => unit.ToString()
    };
}