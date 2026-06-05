using System.Globalization;
using System.Windows.Data;

namespace MeteoForecast.Converters;

public class FavouriteIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isFavourite && isFavourite)
            return "❤";

        return "🤍";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}