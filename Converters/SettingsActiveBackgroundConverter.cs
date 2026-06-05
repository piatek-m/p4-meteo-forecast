using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MeteoForecast.Converters;

public class SettingsActiveBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isActive && isActive)
            return new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));

        return new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}