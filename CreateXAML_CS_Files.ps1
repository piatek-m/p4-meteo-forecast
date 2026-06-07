Get-ChildItem .\Views\*.xaml | ForEach-Object {

    $xamlFile = $_
    $className = $_.BaseName
    $csFile = "$($_.FullName).cs"

@"
using System.Windows;
using System.Windows.Controls;

namespace MeteoForecast.Views;

public partial class $className : UserControl
{
    public $className() => InitializeComponent();
}
"@ | Set-Content $csFile

}