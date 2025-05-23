using System.Windows.Input;

namespace GraphApp
{
    public class Command
    {
        public static RoutedCommand OpenFile { get; set; }
        public static RoutedCommand FindPath {  get; set; }
        public static RoutedCommand DrawGraph {  get; set; }

        static Command()
        {
            FindPath = new RoutedCommand(nameof(FindPath), typeof(MainWindow));
            OpenFile = new RoutedCommand(nameof(OpenFile), typeof(MainWindow));
            DrawGraph = new RoutedCommand(nameof(DrawGraph), typeof(MainWindow));
        }
    }
}
