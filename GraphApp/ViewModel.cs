
/*
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public Graph Graph { get; private set; } = new();
        private Vertex _selectedStartVertex = null!;
        private Vertex _selectedDestinationVertex = null!;
        private string _filePath = string.Empty;
        private bool _isGraphDrawn = false;
        private readonly FileIOService _fileIOService = new();
        private List<Vertex> _shortestPath = new();

        public bool CanFindPath => Graph.Vertices.Count > 0 && 
                                 SelectedStartVertex != null && 
                                 SelectedDestinationVertex != null && 
                                 SelectedStartVertex != SelectedDestinationVertex;

        public FileIOService FileIOService => _fileIOService;

        public bool IsGraphDrawn
        {
            get => _isGraphDrawn;
            set
            {
                _isGraphDrawn = value;
                OnPropertyChanged();
            }
        }

        public Vertex SelectedStartVertex
        {
            get => _selectedStartVertex;
            set
            {
                _selectedStartVertex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanFindPath));
            }
        }

        public Vertex SelectedDestinationVertex
        {
            get => _selectedDestinationVertex;
            set
            {
                _selectedDestinationVertex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanFindPath));
            }
        }

        public string SelectedFilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public List<Vertex> ShortestPath
        {
            get => _shortestPath;
            set
            {
                _shortestPath = value;
                OnPropertyChanged();
            }
        }

        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Select adjacency matrix file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    SelectedFilePath = openFileDialog.FileName;
                    Graph = _fileIOService.ReadGraphFile(SelectedFilePath);
                    MessageBox.Show($"Graph loaded with {Graph.Vertices.Count} vertices.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void FindPath()
        {
            if (!CanFindPath) return;

            var (path, distance) = Graph.FindShortestPath(SelectedStartVertex, SelectedDestinationVertex);
            ShortestPath = path;

            MessageBox.Show($"Shortest path found with distance: {distance}",
                "Path Found", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DrawGraph(Canvas canvas)
        {
            canvas.Children.Clear();
            
            if (Graph.Vertices.Count == 0) return;

            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) * 0.8;
            int vertexCount = Graph.Vertices.Count;

            // Draw edges
            foreach (var edge in Graph.Edges)
            {
                int sourceIdx = Graph.Vertices.IndexOf(edge.Source);
                int destIdx = Graph.Vertices.IndexOf(edge.Destination);

                double angle1 = 2 * Math.PI * sourceIdx / vertexCount;
                double angle2 = 2 * Math.PI * destIdx / vertexCount;

                double x1 = centerX + radius * Math.Sin(angle1);
                double y1 = centerY - radius * Math.Cos(angle1);
                double x2 = centerX + radius * Math.Sin(angle2);
                double y2 = centerY - radius * Math.Cos(angle2);

                var line = new Line
                {
                    X1 = x1, Y1 = y1, X2 = x2, Y2 = y2,
                    Stroke = ShortestPathContains(edge) ? Brushes.Red : Brushes.Gray,
                    StrokeThickness = ShortestPathContains(edge) ? 3 : 1
                };
                canvas.Children.Add(line);

                var text = new TextBlock
                {
                    Text = edge.Weight.ToString(),
                    Background = Brushes.White,
                    FontSize = 10
                };
                Canvas.SetLeft(text, (x1 + x2) / 2);
                Canvas.SetTop(text, (y1 + y2) / 2);
                canvas.Children.Add(text);
            }

            // Draw vertices
            for (int i = 0; i < vertexCount; i++)
            {
                double angle = 2 * Math.PI * i / vertexCount;
                double x = centerX + radius * Math.Sin(angle);
                double y = centerY - radius * Math.Cos(angle);

                var ellipse = new Ellipse
                {
                    Width = 30, Height = 30,
                    Fill = GetVertexBrush(Graph.Vertices[i]),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(ellipse, x - 15);
                Canvas.SetTop(ellipse, y - 15);
                canvas.Children.Add(ellipse);

                var text = new TextBlock
                {
                    Text = Graph.Vertices[i].Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Canvas.SetLeft(text, x - text.ActualWidth / 2);
                Canvas.SetTop(text, y - text.ActualHeight / 2);
                canvas.Children.Add(text);
            }

            IsGraphDrawn = true;
        }

        private bool ShortestPathContains(Edge edge)
        {
            if (ShortestPath == null || ShortestPath.Count < 2) return false;

            for (int i = 0; i < ShortestPath.Count - 1; i++)
            {
                if ((ShortestPath[i] == edge.Source && ShortestPath[i + 1] == edge.Destination) ||
                    (ShortestPath[i] == edge.Destination && ShortestPath[i + 1] == edge.Source))
                    return true;
            }
            return false;
        }

        private Brush GetVertexBrush(Vertex vertex)
        {
            if (vertex == SelectedStartVertex) return Brushes.Green;
            if (vertex == SelectedDestinationVertex) return Brushes.Blue;
            return Brushes.LightBlue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
*/

using DocumentFormat.OpenXml.Drawing;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FileIOService _fileIOService = new();
        private Vertex _selectedStartVertex = null!;
        private Vertex _selectedDestinationVertex = null!;
        private string _filePath = string.Empty;
        private bool _isGraphDrawn = false;
        private ObservableCollection<Vertex> _shortestPath = new();
        private double _shortestDist;
        private Vertex _selectedExclusionVertex = null!;

        public Graph Graph { get; private set; } = new();

        public bool CanFindPath => SelectedStartVertex != null && 
                                 SelectedDestinationVertex != null && 
                                 SelectedStartVertex != SelectedDestinationVertex;

        public bool IsExclusionVertexIncluded => SelectedExclusionVertex != SelectedStartVertex && 
                                SelectedExclusionVertex != SelectedDestinationVertex;

        public FileIOService FileIOService => _fileIOService;
        public double ShortestDistance
        {
            get => _shortestDist;
            set
            {
                _shortestDist = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedStartVertex));
                OnPropertyChanged(nameof(SelectedDestinationVertex));
            }
        }

        public bool IsGraphDrawn
        {
            get => _isGraphDrawn;
            set
            {
                _isGraphDrawn = value;
                OnPropertyChanged();
            }
        }

        public Vertex SelectedStartVertex
        {
            get => _selectedStartVertex;
            set
            {
                _selectedStartVertex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanFindPath));
            }
        }

        public Vertex SelectedDestinationVertex
        {
            get => _selectedDestinationVertex;
            set
            {
                _selectedDestinationVertex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanFindPath));
            }
        }

        public Vertex SelectedExclusionVertex
        {
            get => _selectedExclusionVertex;
            set
            {
                _selectedExclusionVertex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanFindPath));
            }
        }

        public string SelectedFilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Vertex> ShortestPath
        {
            get => _shortestPath;
            set
            {
                _shortestPath = value;
                OnPropertyChanged();
            }
        }

        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Select adjacency matrix file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    SelectedFilePath = openFileDialog.FileName;
                    Graph.Clear(); // Clear existing graph
                    var newGraph = _fileIOService.ReadGraphFile(SelectedFilePath);
                    
                    // Copy vertices and edges to maintain ObservableCollection notifications
                    foreach (var vertex in newGraph.Vertices)
                    {
                        Graph.AddVertex(vertex);
                    }
                    foreach (var edge in newGraph.Edges)
                    {
                        Graph.AddEdge(edge);
                    }

                    MessageBox.Show($"Graph loaded with {Graph.Vertices.Count} vertices.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void FindPath()
        {
            if (!CanFindPath)
            {
                MessageBox.Show("Please select varying vertices.",
                    "Information", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            List<Vertex>? path;
            double distance;

            if (SelectedExclusionVertex != null && IsExclusionVertexIncluded)
            {
                (path, distance) = Graph.FindShortestPath(SelectedStartVertex, SelectedDestinationVertex, SelectedExclusionVertex);
            }
            else
            {
                (path, distance) = Graph.FindShortestPath(SelectedStartVertex, SelectedDestinationVertex);
            }

            if (path == null)
            {
                MessageBox.Show("Path not found. Try again with different vertices.",
                    "Path not found",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            else
            {
                ShortestPath = new ObservableCollection<Vertex>(path);
                ShortestDistance = distance;
                MessageBox.Show($"Shortest path found with distance: {ShortestDistance}",
                "Path Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
  
        public void DrawGraph(Canvas canvas)
        {
            // if (IsGraphDrawn) FindPath();       /// in case the user clicks on draw graph without finding the path first.

            canvas.Children.Clear();
            
            if (Graph.Vertices.Count == 0) return;

            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) * 0.8;
            int vertexCount = Graph.Vertices.Count;

            // Draw edges
            foreach (var edge in Graph.Edges)
            {
                int sourceIdx = Graph.Vertices.IndexOf(edge.Source);
                int destIdx = Graph.Vertices.IndexOf(edge.Destination);

                double angle1 = 2 * Math.PI * sourceIdx / vertexCount;
                double angle2 = 2 * Math.PI * destIdx / vertexCount;

                double x1 = centerX + radius * Math.Sin(angle1);
                double y1 = centerY - radius * Math.Cos(angle1);
                double x2 = centerX + radius * Math.Sin(angle2);
                double y2 = centerY - radius * Math.Cos(angle2);

                bool isInPath = ShortestPathContains(edge);

                var line = new Line
                {
                    X1 = x1, Y1 = y1, X2 = x2, Y2 = y2,
                    Stroke = ShortestPathContains(edge) ? Brushes.Red : Brushes.Gray,
                    StrokeThickness = ShortestPathContains(edge) ? 3 : 1,
                    StrokeDashArray = isInPath ? new DoubleCollection { 2, 2 } : null
                };
                canvas.Children.Add(line);

                var text = new TextBlock
                {
                    Text = edge.Weight.ToString(),
                    Background = Brushes.White,
                    FontSize = 10
                };
                Canvas.SetLeft(text, (x1 + x2) / 2);
                Canvas.SetTop(text, (y1 + y2) / 2);
                canvas.Children.Add(text);
            }

            // Draw vertices
            for (int i = 0; i < vertexCount; i++)
            {
                double angle = 2 * Math.PI * i / vertexCount;
                double x = centerX + radius * Math.Sin(angle);
                double y = centerY - radius * Math.Cos(angle);

                // changes for double highlighting
                var vertex = Graph.Vertices[i];
                bool isInPath = ShortestPath?.Contains(vertex) ?? false;

                var ellipse = new Ellipse
                {
                    Width = 30, Height = 30,
                    Fill = GetVertexBrush(Graph.Vertices[i]),
                    //Stroke = Brushes.Black,
                    //StrokeThickness = 1
                    Stroke = isInPath ? Brushes.Red : Brushes.Black,
                    StrokeThickness = isInPath ? 2 : 1
                };
                Canvas.SetLeft(ellipse, x - 15);
                Canvas.SetTop(ellipse, y - 15);
                canvas.Children.Add(ellipse);

                var text = new TextBlock
                {
                    Text = Graph.Vertices[i].Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = isInPath ? FontWeights.Bold : FontWeights.Normal       /// changes for dh
                };
                Canvas.SetLeft(text, x - text.ActualWidth / 2);
                Canvas.SetTop(text, y - text.ActualHeight / 2);
                canvas.Children.Add(text);
            }

            IsGraphDrawn = true;
        }

        private bool ShortestPathContains(Edge edge)
        {
            if (ShortestPath == null || ShortestPath.Count < 2) return false;

            for (int i = 0; i < ShortestPath.Count - 1; i++)
            {
                if ((ShortestPath[i] == edge.Source && ShortestPath[i + 1] == edge.Destination) ||
                    (ShortestPath[i] == edge.Destination && ShortestPath[i + 1] == edge.Source))
                    return true;
            }
            return false;
        }

        private Brush GetVertexBrush(Vertex vertex)
        {
            if (vertex == SelectedStartVertex) return Brushes.Green;
            if (vertex == SelectedDestinationVertex) return Brushes.Blue;
            if (ShortestPath?.Contains(vertex) ?? false) return Brushes.LightGreen;     /// changes for dh
            return Brushes.LightBlue;
        }

        //
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

