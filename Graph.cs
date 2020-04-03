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
        private readonly List<Node> nodes = new List<Node>();
        private readonly List<Edge> edges = new List<Edge>();

        public Graph(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            for (int i = 0; i < 5; i++)
            {
                AddNode(new int[2] { globalRandom.Next(5), globalRandom.Next(5) });
            }
        }

        public void DrawGraph()
        {
            mainWindow.MainCanvas.Children.Clear();
            Node.ClearID();
            DescribeEdges();
            DrawEdges();
            DrawNodes();
        }

        private void AddNode(int[] connections)
        {
            Node newNode = new Node(connections, globalRandom);
            nodes.Add(newNode);
        }

        private void DescribeEdges()
        {
            foreach(Node currentNode in nodes)
            {
                Node firstNode = currentNode;

                foreach(int nodeID in currentNode.Connections)
                {
                    Node secondNode = nodes.Find(x => x.ID.Equals(nodeID));

                    if(secondNode != null)
                    {
                        edges.Add(new Edge(firstNode.Position, secondNode.Position));
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

            Vector2 position1 = edge.Position1, position2 = edge.Position2;

            drawedEdge.X1 = position1.X;
            drawedEdge.X2 = position2.X;
            drawedEdge.Y1 = position1.Y;
            drawedEdge.Y2 = position2.Y;

            mainWindow.MainCanvas.Children.Add(drawedEdge);
        }

        private void DrawNodes()
        {
            foreach(Node node in nodes)
            {
                DrawSingleNode(node);
            }
        }

        private void DrawEdges()
        {
            foreach(Edge edge in edges)
            {
                DrawSingleEdge(edge);
            }
        }

        public void ConvertToDogs()
        {
            foreach (Node node in nodes)
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
        private int id;
        private int[] connections;
        private readonly MainWindow mainWindow = MainWindow.AppWindow;

        public Vector2 Position;
        public int[] Connections
        {
            get => connections;
            set => connections = value;
        }

        public int ID
        {
            get => id;
            set => id = value;
        }

        public Node(int[] connections, Random globalRandom)
        {
            id = globalID++;
            double canvasWidth = mainWindow.MainCanvas.ActualWidth;
            double canvasHeight = mainWindow.MainCanvas.ActualHeight;
            
            Position.X = globalRandom.Next((int)canvasWidth - 20);
            Position.Y = globalRandom.Next((int)canvasHeight - 20);

            this.connections = connections;
        }

        public static void ClearID()
        {
            globalID = 0;
        }
    }

    class Edge
    {
        public Vector2 Position1, Position2;

        public Edge(Vector2 firstNode, Vector2 secondNode)
        {
            Position1.X = firstNode.X + 10;
            Position2.X = secondNode.X + 10;
            Position1.Y = firstNode.Y + 10;
            Position2.Y = secondNode.Y + 10;
        }
    }
}
