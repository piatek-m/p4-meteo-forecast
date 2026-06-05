using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MeteoForecast.Converters;

public class AlertColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // If val is bool, assign it to hasAlerts and check val of hasAlerts
        if (value is bool hasAlerts && hasAlerts)
            return new SolidColorBrush(Colors.Red);

        return new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}