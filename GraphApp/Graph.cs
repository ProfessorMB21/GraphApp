using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace GraphApp
{
    /*
    public class Graph
    {
        public List<Vertex> Vertices { get; } = new List<Vertex>();
        public List<Edge> Edges { get; } = new List<Edge>();

        public void AddVertex(Vertex vertex) => Vertices.Add(vertex);
        public void AddEdge(Vertex source, Vertex destination, double weight) 
            => Edges.Add(new Edge { Source = source, Destination = destination, Weight = weight });

        public List<Vertex> GetNeighbors(Vertex vertex)
        {
            var neighbors = new List<Vertex>();
            foreach (var edge in Edges)
            {
                if (edge.Source == vertex) neighbors.Add(edge.Destination);
                if (edge.Destination == vertex) neighbors.Add(edge.Source);
            }
            return neighbors;
        }

        public double GetEdgeWeight(Vertex source, Vertex destination)
        {
            foreach (var edge in Edges)
            {
                if ((edge.Source == source && edge.Destination == destination) ||
                    (edge.Source == destination && edge.Destination == source))
                    return edge.Weight;
            }
            return double.PositiveInfinity;
        }

        public (List<Vertex> path, double totalDistance) FindShortestPath(Vertex start, Vertex end)
        {
            // Implementation of Dijkstra's algorithm
            var distances = new Dictionary<Vertex, double>();
            var previous = new Dictionary<Vertex, Vertex>();
            var unvisited = new List<Vertex>();

            foreach (var vertex in Vertices)
            {
                distances[vertex] = vertex == start ? 0 : double.PositiveInfinity;
                previous[vertex] = null!;
                unvisited.Add(vertex);
            }

            while (unvisited.Count > 0)
            {
                var current = unvisited.OrderBy(v => distances[v]).First();
                unvisited.Remove(current);

                if (current == end)
                {
                    var path = new List<Vertex>();
                    while (previous[current] != null)
                    {
                        path.Insert(0, current);
                        current = previous[current];
                    }
                    path.Insert(0, start);
                    return (path, distances[end]);
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!unvisited.Contains(neighbor)) continue;

                    var alt = distances[current] + GetEdgeWeight(current, neighbor);
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            return (null!, double.PositiveInfinity);
        }
    }

}
*/
    public class Graph : INotifyPropertyChanged
    {
        private ObservableCollection<Vertex> _vertices = new();
        private ObservableCollection<Edge> _edges = new();

        public ObservableCollection<Vertex> Vertices
        {
            get => _vertices;
            set
            {
                _vertices = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Edge> Edges
        {
            get => _edges;
            set
            {
                _edges = value;
                OnPropertyChanged();
            }
        }

        public void Clear()
        {
            Vertices.Clear();
            Edges.Clear();
        }

        public void AddVertex(Vertex vertex)
        {
            if (!Vertices.Contains(vertex))
            {
                Vertices.Add(vertex);
            }
        }

        public void AddEdge(Edge edge)
        {
            if (!Edges.Any(e =>
                (e.Source == edge.Source && e.Destination == edge.Destination) ||
                (e.Source == edge.Destination && e.Destination == edge.Source)))
            {
                Edges.Add(edge);
            }
        }

        public void AddEdge(Vertex source, Vertex destination, double weight)
            => AddEdge(new Edge { Source = source, Destination = destination, Weight = weight });

        //

        public List<Vertex> GetNeighbors(Vertex vertex)
        {
            var neighbors = new List<Vertex>();
            foreach (var edge in Edges)
            {
                if (edge.Source == vertex) neighbors.Add(edge.Destination);
                if (edge.Destination == vertex) neighbors.Add(edge.Source);
            }
            return neighbors;
        }

        public double GetEdgeWeight(Vertex source, Vertex destination)
        {
            foreach (var edge in Edges)
            {
                if ((edge.Source == source && edge.Destination == destination) ||
                    (edge.Source == destination && edge.Destination == source))
                    return edge.Weight;
            }
            return double.PositiveInfinity;
        }

        public (List<Vertex> path, double totalDistance) FindShortestPath(Vertex start, Vertex end)
        {
            // Implementation of Dijkstra's algorithm
            var distances = new Dictionary<Vertex, double>();
            var previous = new Dictionary<Vertex, Vertex>();
            var unvisited = new List<Vertex>();

            foreach (var vertex in Vertices)
            {
                distances[vertex] = vertex == start ? 0 : double.PositiveInfinity;
                previous[vertex] = null!;
                unvisited.Add(vertex);
            }

            while (unvisited.Count > 0)
            {
                var current = unvisited.OrderBy(v => distances[v]).First();
                unvisited.Remove(current);

                if (current == end)
                {
                    var path = new List<Vertex>();
                    while (previous[current] != null)
                    {
                        path.Insert(0, current);
                        current = previous[current];
                    }
                    path.Insert(0, start);
                    return (path, distances[end]);
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!unvisited.Contains(neighbor)) continue;

                    var alt = distances[current] + GetEdgeWeight(current, neighbor);
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            return (null!, double.PositiveInfinity);
        }
        public (List<Vertex> path, double totalDistance) FindShortestPath(Vertex start, Vertex end, Vertex? exclude=null)
        {
            // Implementation of Dijkstra's algorithm
            var distances = new Dictionary<Vertex, double>();
            var previous = new Dictionary<Vertex, Vertex>();
            var unvisited = new List<Vertex>();

            foreach (var vertex in Vertices)
            {
                if (vertex != exclude)      // add all vertices exclusive of the excluded vertex
                {
                    distances[vertex] = vertex == start ? 0 : double.PositiveInfinity;
                    previous[vertex] = null!;
                    unvisited.Add(vertex);
                }
            }

            while (unvisited.Count > 0)
            {
                var current = unvisited.OrderBy(v => distances[v]).First();
                unvisited.Remove(current);

                if (current == end)
                {
                    var path = new List<Vertex>();
                    while (previous[current] != null)
                    {
                        path.Insert(0, current);
                        current = previous[current];
                    }
                    path.Insert(0, start);
                    return (path, distances[end]);
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!unvisited.Contains(neighbor) || neighbor == exclude) continue;      // ensure that the excluded vertex is not included as a neighbor

                    var alt = distances[current] + GetEdgeWeight(current, neighbor);
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            return (null!, double.PositiveInfinity);
        }
        //

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
