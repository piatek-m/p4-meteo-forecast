using System.Windows.Controls;
using MeteoForecast.ViewModels;

namespace MeteoForecast.Views;

public partial class SearchView : UserControl
{
    public SearchView()
    {
        InitializeComponent();
        DataContextChanged += (s, e) =>
       {
           if (e.NewValue is SearchViewModel vm)
               vm.PropertyChanged += (_, args) =>
               {
                   if (args.PropertyName == nameof(SearchViewModel.ShouldFocusSearch)
                       && vm.ShouldFocusSearch)
                   {
                       SearchBox.Focus();
                       vm.ShouldFocusSearch = false;
                   }
               };
       };
    }
}