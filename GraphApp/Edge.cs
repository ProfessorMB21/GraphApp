namespace GraphApp
{
    public class Edge
    {
        public Vertex Source { get; set; } = null!;
        public Vertex Destination { get; set; } = null!;
        public double Weight { get; set; }
    }
}