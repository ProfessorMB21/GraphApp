using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace GraphApp
{
    public class FileIOService
    {
        public Graph ReadGraphFile(string filePath)
        {
            return Path.GetExtension(filePath).ToLower() switch
            {
                ".csv" => ReadGraphFromCsv(filePath),
                ".xlsx" or ".xls" => ReadGraphFromExcel(filePath),
                _ => throw new NotSupportedException("Unsupported file format")
            };
        }

        private Graph ReadGraphFromCsv(string filePath)
        {
            var graph = new Graph();
            var lines = File.ReadAllLines(filePath);

            // First line contains vertex names
            var vertexNames = lines[0].Split(',');
            for (int i = 0; i < vertexNames.Length; i++)
            {
                graph.AddVertex(new Vertex { Id = i, Name = vertexNames[i].Trim() });
            }

            // Subsequent lines contain the adjacency matrix
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var sourceVertex = graph.Vertices[i - 1];

                for (int j = 0; j < values.Length; j++)
                {
                    if (double.TryParse(values[j].Trim(), out double weight) && weight > 0)
                    {
                        var destVertex = graph.Vertices[j];
                        graph.AddEdge(sourceVertex, destVertex, weight);
                    }  
                }
            }

            return graph;
        }

        private Graph ReadGraphFromExcel(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            var graph = new Graph();

            // Read vertex names from first row
            var firstRow = worksheet.FirstRowUsed();
            var firstCell = firstRow!.FirstCellUsed();
            var lastCell = firstRow.LastCellUsed();
            int columnCount = lastCell.Address.ColumnNumber - firstCell.Address.ColumnNumber + 1;

            // Add vertices
            for (int col = 1; col <= columnCount; col++)
            {
                var cell = worksheet.Cell(1, col);
                graph.AddVertex(new Vertex { Id = col - 1, Name = cell.GetString().Trim() });
            }

            // Read adjacency matrix
            for (int row = 2; row <= columnCount + 1; row++)
            {
                var sourceVertex = graph.Vertices[row - 2];

                for (int col = 1; col <= columnCount; col++)
                {
                    var cell = worksheet.Cell(row, col);
                    if (cell.TryGetValue(out double weight) && weight > 0)
                    {
                        var destVertex = graph.Vertices[col - 1];
                        graph.AddEdge(sourceVertex, destVertex, weight);
                    }
                }
            }

            return graph;
        }
    }
}
