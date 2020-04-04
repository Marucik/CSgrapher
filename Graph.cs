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
        private List<Edge> Edges { get; } = new List<Edge>();
        public List<Node> Nodes { get; } = new List<Node>();

        public int NodesCount
        {
            get => Nodes.Count;
        }


        public Graph(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            for (int i = 0; i < 10; i++)
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
            Nodes.Add(newNode);
        }

        private void DescribeEdges()
        {
            foreach(Node currentNode in Nodes)
            {
                Node firstNode = currentNode;

                foreach(int nodeID in currentNode.Connections)
                {
                    Node secondNode = Nodes.Find(x => x.ID.Equals(nodeID));

                    if(secondNode != null)
                    {
                        Edges.Add(new Edge(firstNode, secondNode));
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
            foreach(Node node in Nodes)
            {
                DrawSingleNode(node);
            }
        }

        private void DrawEdges()
        {
            foreach(Edge edge in Edges)
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
        private int id;
        private int[] connections;
        private readonly MainWindow mainWindow = MainWindow.AppWindow;
        public Vector2 Position;
        public Vector2 Displacement;

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
            
            Position.X = globalRandom.Next((int)canvasWidth);
            Position.Y = globalRandom.Next((int)canvasHeight);

            this.connections = connections;
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
