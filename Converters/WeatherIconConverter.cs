using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MeteoForecast.Converters;

public class WeatherIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int code) return "👽";

        return code switch
        {
            0 => "☀",                      // Sunny
            1 => "🌤",                      // Mostly sunny
            2 => "⛅",                     // Partial overcast
            3 => "☁",                      // Overcast
            45 or 48 => "🌫",               // Fog
            51 or 53 or 55 => "🌦",         // Light rain
            61 or 63 or 65 => "🌧",         // Rain
            71 or 73 or 75 or 77 => "❄",   // Snow
            80 or 81 or 82 => "☔",        // Showers (przelotne opady)
            85 or 86 => "🌨",               // Snow showers
            95 => "🌩",                     // Storm
            96 or 99 => "⛈",               // Hailstorm (gradobicie)
            _ => "👽"                      // Unkown code
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}