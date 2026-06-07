using MeteoForecast.Services.Interfaces;
using MeteoForecast.ViewModels.Generics;
using Microsoft.Extensions.DependencyInjection;

namespace MeteoForecast.ViewModels;

public class ShellViewModel : BaseViewModel, INavigationService
{
    private readonly IServiceProvider _services;

    public SearchViewModel SearchViewModel => _services.GetRequiredService<SearchViewModel>();

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
        _currentViewModel = null!;

        GoToSearchCommand = new RelayCommand(_ => NavigateTo<SearchViewModel>());
        GoToSettingsCommand = new RelayCommand(_ => ToggleSettings());
        GoBackCommand = new RelayCommand(_ => GoBack(), _ => _previousViewModel is not null);
        ToggleAlertsCommand = new RelayCommand(_ => IsAlertsOpen = !IsAlertsOpen);
        CloseAlertsCommand = new RelayCommand(_ => IsAlertsOpen = false);
    }

    public void Initialize()
    {
        NavigateTo<MainViewModel>();
    }

    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        var vm = _services.GetRequiredService<TViewModel>();
        if (CurrentViewModel == vm) return;

        var previous = CurrentViewModel;
        _previousViewModel = CurrentViewModel;
        CurrentViewModel = vm;
        IsAlertsOpen = false;

        if (vm is MainViewModel mainVm)
            mainVm.OnNavigatedToFrom(previous);
        else
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

        var previous = CurrentViewModel;
        _previousViewModel = CurrentViewModel;
        CurrentViewModel = vm;
        IsAlertsOpen = false;

        if (vm is MainViewModel mainVm)
            mainVm.OnNavigatedToFrom(previous);
        else
            vm.OnNavigatedTo();
        GoBackCommand.RaiseCanExecuteChanged();
    }

    public void GoBack()
    {
        if (_previousViewModel is null) return;

        var previous = CurrentViewModel;
        CurrentViewModel = _previousViewModel;
        _previousViewModel = null;

        if (CurrentViewModel is MainViewModel mainVm)
            mainVm.OnNavigatedToFrom(previous);
        else
            CurrentViewModel.OnNavigatedTo();
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