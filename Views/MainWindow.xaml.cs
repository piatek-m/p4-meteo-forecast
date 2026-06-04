using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MeteoForecast.ViewModels;

namespace MeteoForecast.Views;

public partial class MainWindow : Window
{
    public MainWindow(ShellViewModel shellViewModel)
    {
        InitializeComponent();
        DataContext = shellViewModel;
    }
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            var shell = (ShellViewModel)DataContext;
            if (shell.IsAlertsOpen)
                shell.CloseAlertsCommand.Execute(null);
            else if (shell.IsSettingsActive)
                shell.GoBackCommand.Execute(null);
        }
        base.OnKeyDown(e);
    }
}
