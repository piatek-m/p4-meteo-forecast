# MeteoForecast

Aplikacja pogodowa WPF korzystająca z API [Open-Meteo](https://open-meteo.com/).

## Wykorzystane paczki

- [Microsoft.Extensions.Hosting](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting?view=net-10.0-pp)
- [Microsoft.EntityFrameworkCore.Design](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.design?view=efcore-10.0)
- [Microsoft.EntityFrameworkCore.Sqlite](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-core-cli)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [Microsoft.Extensions.Http](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.http?view=net-10.0-pp)

## Funkcjonalności
- Wyszukiwanie miast poprzez [NominatimApi](https://nominatim.org/)
- Automatyczne wykrywanie lokalizacji [Windows.Devices.Geolocation](https://learn.microsoft.com/en-us/uwp/api/windows.devices.geolocation?view=winrt-28000)
- ~~Ręczne dodawanie miejscowości poprzez współrzędne geograficzne~~ niezrobione
- Historia wyszukiwań & ulubione miejscowości
- Widok z podziałem na godziny (ikonka, temp., temp. odczuwalna, opady, ciśnienie, wiatr, etc.)
- Cache pogody (rzadsze pobieranie z API)
- ~~Alerty (temp. wysoka/niska, wysoki wiatr, ciśnienie)~~ niezrobione
- Ustawienia aplikacji (~~progi alertów~~, jednostki)
