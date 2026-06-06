using System.IO;
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
        File.AppendAllText("debug.log", "MainWindow ctor\n");
    }

    private void SearchBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (DataContext is ShellViewModel shell
            && shell.CurrentViewModel is not SearchViewModel)
            shell.NavigateTo<SearchViewModel>();
    }

    private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return
            && DataContext is ShellViewModel shell
            && shell.CurrentViewModel is not SearchViewModel)
            shell.NavigateTo<SearchViewModel>();
    }
}

