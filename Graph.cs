using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using CSgrapher;
using System.Windows.Media.Imaging;
using System.Numerics;
using Helpers;

namespace Graph
{
    class Graph
    {
        private readonly Random globalRandom = new Random();
        private readonly MainWindow mainWindow;
        private readonly List<List<int>> adjacencyMatrix = new List<List<int>>();

        public List<Edge> Edges { get; } = new List<Edge>();

        public List<Node> Nodes { get; } = new List<Node>();

        public string AdjecencyMatrixString
        {
            get => GenerateGraphString();
        }

        public int NodesCount
        {
            get => Nodes.Count;
        }


        public Graph(MainWindow mainWindow, int nodesCount)
        {
            this.mainWindow = mainWindow;
            GenerateNodes(nodesCount);
            GenerateConnections();
            DescribeEdges();
        }

        public Graph(MainWindow mainWindow, List<List<int>> adjacencyMatrix)
        {
            this.mainWindow = mainWindow;
            this.adjacencyMatrix = adjacencyMatrix;
            GenerateNodes(adjacencyMatrix.Count);
            DescribeEdges();
        }

        public Graph(MainWindow mainWindow, string adjacencyMatrixString)
        {
            this.mainWindow = mainWindow;
            adjacencyMatrix = ConvertToMatrix(adjacencyMatrixString);
            GenerateNodes(adjacencyMatrix.Count);
            DescribeEdges();
        }

        public void DrawGraph()
        {
            mainWindow.MainCanvas.Children.Clear();
            Node.ClearID();
            DrawEdges();
            DrawNodes();

            mainWindow.sequentialProggresBar.Value = 0;
            mainWindow.EdgesCount.Content = Edges.Count;
        }

        public void DrawTidyGraph()
        {
            mainWindow.MainCanvas.Children.Clear();
            DrawEdges();
            DrawNodes();
        }

        public void HighlightNodeAndEdges(Point mousePosition)
        {
            DrawGraph();

            foreach (Node node in Nodes)
            {
                float nodeX = node.Position.X;
                float nodeY = node.Position.Y;

                if (mousePosition.X >= nodeX && mousePosition.X <= nodeX + 20)
                {
                    if (mousePosition.Y >= nodeY && mousePosition.Y <= nodeY + 20)
                    {
                        DrawHighlightedEdges(node);
                        DrawHighlightedNode(node);
                        break;
                    }
                }
            }
        }

        private void GenerateNodes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Node newNode = new Node(globalRandom);
                Nodes.Add(newNode);
            }
        }

        private void GenerateConnections()
        {
            foreach (Node currentNode in Nodes)
            {
                List<int> neightbours = new List<int>();

                for (int i = 0; i <= currentNode.ID; i++)
                {
                    if (i == currentNode.ID)
                    {
                        neightbours.Add(0);
                    }
                    else
                    {
                        neightbours.Add((int)Math.Round(globalRandom.NextDouble()));
                    }
                }

                adjacencyMatrix.Add(neightbours);

                for (int i = 0; i < adjacencyMatrix.Count; i++)
                {
                    for (int j = 0; j < adjacencyMatrix[i].Count; j++)
                    {
                        if (adjacencyMatrix[i][j] == 1)
                        {
                            Node neighbour = Nodes.Find(x => x.ID.Equals(j));
                            Nodes[i].Connections.Add(neighbour);
                        }
                    }
                }
            }
        }

        private void DescribeEdges()
        {
            for (int i = 0; i < adjacencyMatrix.Count; i++)
            {
                for (int j = 0; j < adjacencyMatrix[i].Count; j++)
                {
                    if (adjacencyMatrix[i][j] == 1)
                    {
                        Node neighbour = Nodes.Find(x => x.ID.Equals(j));

                        Edge newEdge = new Edge(Nodes[i], neighbour);

                        Edges.Add(newEdge);
                    }
                }
            }
        }

        private void DrawSingleNode(Node node)
        {
            Vector2 position = node.Position;

            Ellipse drawedNode = new Ellipse
            {
                Fill = Brushes.GreenYellow,
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Width = 20,
                Height = 20,
                Margin = new Thickness(position.X, position.Y, 0, 0)
            };

            Canvas.SetZIndex(drawedNode, 3);

            mainWindow.MainCanvas.Children.Add(drawedNode);
        }

        private void DrawHighlightedNode(Node node)
        {
            Vector2 position = node.Position;

            Ellipse drawedNode = new Ellipse
            {
                Fill = Brushes.Red,
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Width = 20,
                Height = 20,
                Margin = new Thickness(position.X, position.Y, 0, 0)
            };

            Canvas.SetZIndex(drawedNode, 4);

            mainWindow.MainCanvas.Children.Add(drawedNode);
        }

        private void DrawSingleEdge(Edge edge)
        {
            Vector2 position1 = edge.Nodes[0].Position;
            Vector2 position2 = edge.Nodes[1].Position;

            Line drawedEdge = new Line
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                X1 = position1.X + 10,
                X2 = position2.X + 10,
                Y1 = position1.Y + 10,
                Y2 = position2.Y + 10
            };

            Canvas.SetZIndex(drawedEdge, 1);

            mainWindow.MainCanvas.Children.Add(drawedEdge);
        }

        private void DrawHighlightedEdge(Edge edge)
        {
            Vector2 position1 = edge.Nodes[0].Position;
            Vector2 position2 = edge.Nodes[1].Position;

            Line drawedEdge = new Line
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                X1 = position1.X + 10,
                X2 = position2.X + 10,
                Y1 = position1.Y + 10,
                Y2 = position2.Y + 10
            };

            Canvas.SetZIndex(drawedEdge, 2);

            mainWindow.MainCanvas.Children.Add(drawedEdge);
        }

        private void DrawNodes()
        {
            foreach (Node node in Nodes)
            {
                DrawSingleNode(node);
            }
        }

        private void DrawEdges()
        {
            foreach (Edge edge in Edges)
            {
                DrawSingleEdge(edge);
            }
        }

        private void DrawHighlightedEdges(Node node)
        {
            foreach (Edge edge in Edges)
            {
                if (edge.Nodes[0].ID == node.ID || edge.Nodes[1].ID == node.ID)
                {
                    DrawHighlightedEdge(edge);
                }
            }
        }

        private string GenerateGraphString()
        {
            string matrixString = "";

            foreach (List<int> row in adjacencyMatrix)
            {
                foreach (int connection in row)
                {
                    matrixString += $"{connection} ";
                }
                matrixString += "\n";
            }

            return matrixString;
        }

        private List<List<int>> ConvertToMatrix(string adjacencyMatrixString)
        {
            List<List<int>> newAdjecencyMatrix = new List<List<int>>();

            string[] rows = adjacencyMatrixString.Split(" \n");

            foreach (string row in rows)
            {
                if (row != "")
                {
                    string[] connections = row.Length != 1 ? row.Split(" ") : row.Split("");
                    List<int> connectionInts = new List<int>();

                    foreach (string edge in connections)
                    {
                        connectionInts.Add(Int32.Parse(edge));
                    }

                    newAdjecencyMatrix.Add(connectionInts);
                }
            }

            return newAdjecencyMatrix;
        }

        public void ConvertToDogs()
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("https://pngimage.net/wp-content/uploads/2018/05/dog-face-png-2.png");
            bi3.EndInit();

            foreach (Node node in Nodes)
            {
                Vector2 position = node.Position;
                Image doggie = new Image
                {
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(position.X - 10, position.Y - 10, 0, 0),
                    Source = bi3
                };

                mainWindow.MainCanvas.Children.Add(doggie);
            }
        }
    }

    class Node
    {
        private static int globalID = 0;
        private readonly MainWindow mainWindow = MainWindow.AppWindow;
        public Vector2 Position;
        public Vector2 Displacement;

        public List<Node> Connections { get; set; } = new List<Node>();

        public int ID { get; set; }

        public Node(Random globalRandom)
        {
            ID = globalID++;
            double shrinkedCanvasWidth = mainWindow.MainCanvas.ActualWidth - 20;
            double shrinkedCanvasHeight = mainWindow.MainCanvas.ActualHeight - 20;

            Position.X = globalRandom.NextFloatRange(0, (float)shrinkedCanvasWidth);
            Position.Y = globalRandom.NextFloatRange(0, (float)shrinkedCanvasHeight);
        }

        public static void ClearID()
        {
            globalID = 0;
        }
    }

    class Edge
    {
        public List<Node> Nodes { get; }

        public Edge(Node firstNode, Node secondNode)
        {
            Nodes = new List<Node> { firstNode, secondNode };
        }
    }
}
