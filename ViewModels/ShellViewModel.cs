using System.Security.Cryptography.X509Certificates;
using MeteoForecast.ViewModels.Generics;

namespace MeteoForecast.ViewModels;

public class ShellViewModel : BaseViewModel
{
    private readonly MainViewModel _mainViewModel;
    private readonly SearchViewModel _searchViewModel;
    private readonly SettingsViewModel _settingsViewModel;
    private readonly AlertsViewModel _alertsViewModel;

    private BaseViewModel _currentViewModel;
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }
    private BaseViewModel? _previousViewModel;

    private bool _isAlertsOpen;
    public bool IsAlertsOpen
    {
        get => _isAlertsOpen;
        private set => SetProperty(ref _isAlertsOpen, value);
    }
    public bool IsSettingsActive => CurrentViewModel is SettingsViewModel;

    public RelayCommand GoToSearchCommand { get; }
    public RelayCommand GoToSettingsCommand { get; }
    public RelayCommand GoBackCommand { get; }
    public RelayCommand ToggleAlertsCommand { get; }
    public RelayCommand CloseAlertsCommand { get; }

    public ShellViewModel(
        MainViewModel mainViewModel,
        SearchViewModel searchViewModel,
        SettingsViewModel settingsViewModel,
        AlertsViewModel alertsViewModel
        )
    {
        _mainViewModel = mainViewModel;
        _searchViewModel = searchViewModel;
        _settingsViewModel = settingsViewModel;
        _alertsViewModel = alertsViewModel;

        _currentViewModel = _mainViewModel;

        GoToSearchCommand = new RelayCommand(_ => NavigateTo(_searchViewModel));
        GoToSettingsCommand = new RelayCommand(_ => ToggleSettings());
        GoBackCommand = new RelayCommand(_ => GoBack(), _ => _previousViewModel is not null);
        ToggleAlertsCommand = new RelayCommand(_ => ToggleAlerts());
        CloseAlertsCommand = new RelayCommand(_ => IsAlertsOpen = false);
    }
    public void NavigateTo(BaseViewModel destination)
    {
        if (CurrentViewModel == destination) return;

        _previousViewModel = CurrentViewModel;
        CurrentViewModel = destination;
        IsAlertsOpen = false;

        OnPropertyChanged(nameof(IsSettingsActive));

        destination.OnNavigatedTo();
    }
    private void ToggleSettings()
    {
        if (CurrentViewModel is SettingsViewModel)
            GoBack();
        else
            NavigateTo(_settingsViewModel);

        OnPropertyChanged(nameof(IsSettingsActive));
    }
    private void ToggleAlerts()
    {
        IsAlertsOpen = !IsAlertsOpen;
    }
    private void GoBack()
    {
        if (_previousViewModel is null) return;

        CurrentViewModel = _previousViewModel;
        _previousViewModel = null;

        OnPropertyChanged(nameof(IsSettingsActive));
        GoBackCommand.RaiseCanExecuteChanged();
    }
}