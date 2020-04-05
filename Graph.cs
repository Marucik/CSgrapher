using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using CSgrapher;
using System.Windows.Media.Imaging;
using System.Numerics;

namespace Graph
{
    class Graph
    {
        private readonly Random globalRandom = new Random();
        private readonly MainWindow mainWindow;
        private List<List<int>> adjacencyMatrix = new List<List<int>>();

        private List<Edge> Edges { get; } = new List<Edge>();

        public List<Node> Nodes { get; } = new List<Node>();

        public int NodesCount
        {
            get => Nodes.Count;
        }


        public Graph(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            GenerateNodes(10);
            GenerateConnections();
        }

        public void DrawGraph()
        {
            mainWindow.MainCanvas.Children.Clear();
            Node.ClearID();
            DescribeEdges();
            DrawEdges();
            DrawNodes();
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
            Ellipse drawedNode = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush
            {
                Color = Color.FromArgb(255, 255, 150, 150)
            };

            drawedNode.Fill = mySolidColorBrush;
            drawedNode.StrokeThickness = 1;
            drawedNode.Stroke = Brushes.Black;

            drawedNode.Width = 20;
            drawedNode.Height = 20;

            Vector2 position = node.Position;

            drawedNode.Margin = new Thickness(position.X, position.Y, 0, 0);

            mainWindow.MainCanvas.Children.Add(drawedNode);
        }

        private void DrawSingleEdge(Edge edge)
        {
            Line drawedEdge = new Line
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black
            };

            Vector2 position1 = edge.Nodes[0].Position;
            Vector2 position2 = edge.Nodes[1].Position;

            drawedEdge.X1 = position1.X + 10;
            drawedEdge.X2 = position2.X + 10;
            drawedEdge.Y1 = position1.Y + 10;
            drawedEdge.Y2 = position2.Y + 10;

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


        public void ConvertToDogs()
        {
            foreach (Node node in Nodes)
            {
                Image doggie = new Image();
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("https://pngimage.net/wp-content/uploads/2018/05/dog-face-png-2.png");
                bi3.EndInit();

                doggie.Width = 40;
                doggie.Height = 40;

                Vector2 position = node.Position;

                doggie.Margin = new Thickness(position.X - 10, position.Y - 10, 0, 0);

                doggie.Source = bi3;

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
            double canvasWidth = mainWindow.MainCanvas.ActualWidth;
            double canvasHeight = mainWindow.MainCanvas.ActualHeight;

            Position.X = globalRandom.Next((int)canvasWidth);
            Position.Y = globalRandom.Next((int)canvasHeight);
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
