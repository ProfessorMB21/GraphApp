using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    MainViewModel vm = new();
    private readonly string _title = "PathFinder🛣️";
    public MainWindow()
    {
        InitializeComponent();
        DataContext = vm;
    }
    private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        vm.Clear();
        vm.OpenFile();
        statBar.Visibility = Visibility.Visible;
        resetSelectionsBtn.IsEnabled = true;
        vm.DrawGraph(GraphCanvas);
    }
    private void FindPath_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if ((vm.IsGraphDrawn || !vm.IsGraphDrawn) && vm.CanFindPath)
        {
            gBoxPathInfo.Visibility = Visibility.Visible;
        }
        vm.FindPath();
        vm.DrawGraph(GraphCanvas);
        Title = $"{_title}: {vm.PathToString()}";
    }

    /// new feature
    private void ResetButton_Clicked(object sender, RoutedEventArgs e)
    {
        vm.SelectedStartVertex = null!;
        vm.SelectedDestinationVertex = null!;
        vm.SelectedExclusionVertex = null!;
    }
}
