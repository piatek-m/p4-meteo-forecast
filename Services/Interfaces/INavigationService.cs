using MeteoForecast.ViewModels.Generics;

namespace MeteoForecast.Services.Interfaces;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    void NavigateTo<TViewModel>(Action<TViewModel> configure) where TViewModel : BaseViewModel;
    void GoBack();
}