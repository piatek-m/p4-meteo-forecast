# MeteoForecast

Aplikacja pogodowa WPF korzystająca z API [Open-Meteo](https://open-meteo.com/).

## Wykorzystane paczki

- [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/microsoft.extensions.hosting/)
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/microsoft.entityframeworkcore.design/)
- [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/microsoft.entityframeworkcore.sqlite)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)

## Planowane funkcjonalności
- Wyszukiwanie miast poprzez [NominatimApi](https://nominatim.org/)
- Automatyczne wykrywanie lokalizacji [Windows.Devices.Geolocation](https://learn.microsoft.com/en-us/uwp/api/windows.devices.geolocation?view=winrt-28000)
- Ręczne dodawanie miejscowości poprzez współrzędne geograficzne
- Historia wyszukiwań & ulubione miejscowości
- Widok z podziałem na godziny (ikonka, temp., temp. odczuwalna, opady, ciśnienie, wiatr, etc.)
- Cache pogody (rzadsze pobieranie z API)
- Alerty (temp. wysoka/niska, wysoki wiatr, ciśnienie)
- Ustawienia aplikacji (progi alertów, jednostki)