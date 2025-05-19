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
    public MainWindow()
    {
        InitializeComponent();
        DataContext = vm;
    }
    private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        vm.OpenFile();
        statBar.Visibility = Visibility.Visible;
    }
    private void DrawGraph_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (vm.IsGraphDrawn)
        {
            gBoxPathInfo.Visibility = Visibility.Visible;
        }
        vm.DrawGraph(GraphCanvas);
    }
    private void FindPath_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (vm.IsGraphDrawn || !vm.IsGraphDrawn)
        {
            gBoxPathInfo.Visibility = Visibility.Visible;
        }
        vm.FindPath();
    }

}
