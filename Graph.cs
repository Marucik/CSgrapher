using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using CSgrapher;

namespace Graph
{
    class Graph
    {
        private Random globalRandom = new Random();
        private MainWindow mainWindow;
        private List<Node> nodes = new List<Node>();

        public Graph(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void AddNode(int[] connections)
        {
            Node newNode = new Node(connections, globalRandom);
            nodes.Add(newNode);
        }

        public void DrawNodes()
        {
            foreach(Node node in nodes)
            {
                Ellipse drawedNode = new Ellipse();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);

                drawedNode.Fill = mySolidColorBrush;
                drawedNode.StrokeThickness = 2;
                drawedNode.Stroke = Brushes.Black;

                drawedNode.Width = 20;
                drawedNode.Height = 20;

                drawedNode.Margin = new Thickness(node.X, node.Y, 0, 0);

                

                mainWindow.MainCanvas.Children.Add(drawedNode);
            }
        }
    }
    
    // 000001
    // PL1000PLN04024
    // 
    class Node
    {
        private static int globalID = 0;
        private int id;
        private int[] connections;
        public int X, Y;
        private MainWindow mainWindow = MainWindow.AppWindow;

        public Node(int[] connections, Random globalRandom)
        {
            id = globalID++;
            double canvasWidth = mainWindow.MainCanvas.ActualWidth;
            double canvasHeight = mainWindow.MainCanvas.ActualHeight;
            
            X = globalRandom.Next((int)canvasWidth);
            Y = globalRandom.Next((int)canvasHeight);

            this.connections = connections;
        }
    }

    class Edge
    {

    }
}
