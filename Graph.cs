using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using CSgrapher;
using System.Windows.Media.Imaging;

namespace Graph
{
    class Graph
    {
        private Random globalRandom = new Random();
        private MainWindow mainWindow;
        private List<Node> nodes = new List<Node>();
        private List<Edge> edges = new List<Edge>();

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

                doggie.Margin = new Thickness(node.X - 10, node.Y - 10, 0, 0);

                doggie.Source = bi3;

                mainWindow.MainCanvas.Children.Add(doggie);
            }
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
                        edges.Add(new Edge(firstNode, secondNode));
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

            drawedNode.Margin = new Thickness(node.X, node.Y, 0, 0);

            mainWindow.MainCanvas.Children.Add(drawedNode);
        }

        private void DrawSingleEdge(Edge edge)
        {
            Line drawedEdge = new Line();

            drawedEdge.StrokeThickness = 1;
            drawedEdge.Stroke = Brushes.Black;

            drawedEdge.X1 = edge.X1;
            drawedEdge.X2 = edge.X2;
            drawedEdge.Y1 = edge.Y1;
            drawedEdge.Y2 = edge.Y2;

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
    }
    
    class Node
    {
        private static int globalID = 0;
        private int id;
        private int[] connections;
        private MainWindow mainWindow = MainWindow.AppWindow;

        public int X, Y;
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
            
            X = globalRandom.Next((int)canvasWidth - 20);
            Y = globalRandom.Next((int)canvasHeight - 20);

            this.connections = connections;
        }

        public static void ClearID()
        {
            globalID = 0;
        }
    }

    class Edge
    {
        public int X1, X2, Y1, Y2;

        public Edge(Node firstNode, Node secondNode)
        {
            X1 = firstNode.X + 10;
            X2 = secondNode.X + 10;
            Y1 = firstNode.Y + 10;
            Y2 = secondNode.Y + 10;
        }
    }
}
