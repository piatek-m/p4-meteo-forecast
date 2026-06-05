using MeteoForecast.Models;

namespace MeteoForecast.Converters;

public static class WindConverter
{
    private static readonly string[] Cardinals =
        ["N", "NE", "E", "SE", "S", "SW", "W", "NW"];

    private static readonly string[] Arrows =
        ["↑", "↗", "→", "↘", "↓", "↙", "←", "↖"];

    public static string Convert(int degrees, WindDirectionDisplay display)
    {
        var normalized = ((degrees % 360) + 360) % 360;

        // 8 directions -> direction changes every 45 degs
        var index = (int)((normalized + 22.5) / 45) % 8;

        return display switch
        {
            WindDirectionDisplay.Cardinal => Cardinals[index],
            WindDirectionDisplay.Arrow => Arrows[index],
            _ => throw new ArgumentOutOfRangeException(nameof(display))
        };
    }
}