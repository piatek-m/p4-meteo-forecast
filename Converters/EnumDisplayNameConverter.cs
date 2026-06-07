using System.Globalization;
using System.Windows.Data;
using MeteoForecast.Extensions;
using MeteoForecast.Models;

namespace MeteoForecast.Converters;

public class EnumDisplayNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            WindSpeedUnit u => u.DisplayName(),
            TemperatureUnit u => u.DisplayName(),
            PressureUnit u => u.DisplayName(),
            WindDirectionDisplay u => u.DisplayName(),
            _ => value?.ToString() ?? ""
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}