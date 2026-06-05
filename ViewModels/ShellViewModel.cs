using System.Security.Cryptography.X509Certificates;
using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Generics;
using MeteoForecast.Views;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace MeteoForecast.ViewModels;

public class ShellViewModel : BaseViewModel, INavigationService
{
    private readonly IServiceProvider _services;

    public AlertsViewModel AlertsViewModel => _services.GetRequiredService<AlertsViewModel>();

    private BaseViewModel _currentViewModel = null!;
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            SetProperty(ref _currentViewModel, value);
            OnPropertyChanged(nameof(IsSettingsActive));
        }
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

    public ShellViewModel(IServiceProvider services)
    {
        _services = services;
        _currentViewModel = _services.GetRequiredService<MainViewModel>();

        GoToSearchCommand = new RelayCommand(_ => NavigateTo<SearchViewModel>());
        GoToSettingsCommand = new RelayCommand(_ => ToggleSettings());
        GoBackCommand = new RelayCommand(_ => GoBack(), _ => _previousViewModel is not null);
        ToggleAlertsCommand = new RelayCommand(_ => IsAlertsOpen = !IsAlertsOpen);
        CloseAlertsCommand = new RelayCommand(_ => IsAlertsOpen = false);
    }

    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        var vm = _services.GetRequiredService<TViewModel>();
        if (CurrentViewModel == vm) return;

        _previousViewModel = CurrentViewModel;
        CurrentViewModel = vm;
        IsAlertsOpen = false;

        vm.OnNavigatedTo();
        GoBackCommand.RaiseCanExecuteChanged();
    }

    public void NavigateTo<TViewModel>(Action<TViewModel> configure) where TViewModel : BaseViewModel
    {
        var vm = _services.GetRequiredService<TViewModel>();
        configure(vm);

        if (CurrentViewModel == vm)
        {
            vm.OnNavigatedTo();
            return;
        }

        _previousViewModel = CurrentViewModel;
        CurrentViewModel = vm;
        IsAlertsOpen = false;

        vm.OnNavigatedTo();
        GoBackCommand.RaiseCanExecuteChanged();
    }

    public void GoBack()
    {
        if (_previousViewModel is null) return;

        CurrentViewModel = _previousViewModel;
        _previousViewModel = null;

        GoBackCommand.RaiseCanExecuteChanged();
    }

    private void ToggleSettings()
    {
        if (CurrentViewModel is SettingsViewModel)
        {
            var settingsVm = _services.GetRequiredService<SettingsViewModel>();
            _ = settingsVm.SaveAsync();
            GoBack();
        }
        else
        {
            NavigateTo<SettingsViewModel>();
        }
    }
}