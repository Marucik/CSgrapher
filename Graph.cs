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
using System.Windows.Documents;

namespace Graph
{
    /// <summary>
    /// Class representing single graph instance. It contains methods for creating random, generated from string or pseudo matrix graphs.
    /// </summary>
    class Graph
    {
        private readonly System.Random globalRandom = new System.Random();
        private readonly MainWindow mainWindow;
        private readonly List<List<int>> adjacencyMatrix = new List<List<int>>();

        /// <summary>
        /// Property which stores <see cref="List"/> of current <see cref="Graph"/> <see cref="Edge"/>s.
        /// </summary>
        public List<Edge> Edges { get; } = new List<Edge>();

        /// <summary>
        /// Property which stores <see cref="List"/> of current <see cref="Graph"/> <see cref="Node"/>s.
        /// </summary>
        public List<Node> Nodes { get; } = new List<Node>();

        /// <summary>
        /// Property which returns <see cref="string"/> with current <see cref="Graph"/> in pseudo matrix form.
        /// </summary>
        /// <example>
        /// Result:
        /// 0
        /// 0 0
        /// 0 1 0
        /// 1 0 0 1
        /// 0 0 1 1 1    
        /// </example>
        public string AdjecencyMatrixString
        {
            get => GenerateGraphString();
        }

        /// <summary>
        /// Property which returns number of <see cref="Node"/>s
        /// </summary>
        public int NodesCount
        {
            get => Nodes.Count;
        }

        /// <summary>
        /// Basic constructor which generates random graph based on <paramref name="nodesCount"/> param.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="nodesCount"></param>
        public Graph(MainWindow mainWindow, int nodesCount)
        {
            this.mainWindow = mainWindow;

            Node.ClearID();
            GenerateNodes(nodesCount);
            GenerateConnections();
            DescribeEdges();
        }

        /// <summary>
        /// Constructor which generates graph with connectins based on <paramref name="adjacencyMatrix"/> pseudo matrix param.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="adjacencyMatrix"></param>
        public Graph(MainWindow mainWindow, List<List<int>> adjacencyMatrix)
        {
            this.mainWindow = mainWindow;

            Node.ClearID();
            this.adjacencyMatrix = adjacencyMatrix;
            GenerateNodes(adjacencyMatrix.Count);
            DescribeEdges();
        }

        /// <summary>
        /// Constructor which generates graph with connectins based on <paramref name="adjacencyMatrixString"/> string param.
        /// Used to recreate graph form file.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="adjacencyMatrixString"></param>
        public Graph(MainWindow mainWindow, string adjacencyMatrixString)
        {
            this.mainWindow = mainWindow;

            Node.ClearID();
            adjacencyMatrix = ConvertToMatrix(adjacencyMatrixString);
            GenerateNodes(adjacencyMatrix.Count);
            DescribeEdges();
        }

        /// <summary>
        /// Method for drawing graph on target <see cref="Canvas"/>.
        /// </summary>
        public void DrawGraph()
        {
            mainWindow.MainCanvas.Children.Clear();
            DrawEdges();
            DrawNodes();

            mainWindow.sequentialProggresBar.Value = 0;
            mainWindow.EdgesCount.Content = Edges.Count;
        }

        /// <summary>
        /// Method which highlights <see cref="Node"/> and corresponding <see cref="Edge"/>s on click.
        /// It gets position of mouse on <see cref="Canvas"/> and check if there is any <see cref="Node"/>.
        /// If true, it changes <see cref="Node"/> color and make <see cref="Edge"/>s thicker.
        /// </summary>
        /// <param name="mousePosition"></param>
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

        /// <summary>
        /// Method which generates given number of randomly placed nodes.
        /// </summary>
        /// <param name="count"></param>
        private void GenerateNodes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Node newNode = new Node(globalRandom, mainWindow);
                Nodes.Add(newNode);
            }
        }

        /// <summary>
        /// Method which adds random connections to existing <see cref="Nodes"/>.
        /// It also creates <see cref="adjacencyMatrix"/>.
        /// </summary>
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

        /// <summary>
        /// Method which create "graphical" connections between <see cref="Nodes"/>.
        /// </summary>
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

        /// <summary>
        /// Method for drawing <see cref="Node"/> on <see cref="Canvas"/>.
        /// </summary>
        /// <param name="node"></param>
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

            Label nodeID = new Label
            {
                Padding = new Thickness(0, 0, 0, 0),
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(position.X, position.Y, 0, 0),
                Content = node.ID
            };

            Canvas.SetZIndex(nodeID, 6);


            mainWindow.MainCanvas.Children.Add(drawedNode);
            mainWindow.MainCanvas.Children.Add(nodeID);
        }

        /// <summary>
        /// Method for drawing highlighted <see cref="Node"/> on <see cref="Canvas"/>.
        /// <see cref="HighlightNodeAndEdges(Point)"/>
        /// </summary>
        /// <param name="node"></param>
        private void DrawHighlightedNode(Node node)
        {
            Vector2 position = node.Position;

            Ellipse drawedNode = new Ellipse
            {
                Fill = Brushes.CadetBlue,
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Width = 20,
                Height = 20,
                Margin = new Thickness(position.X, position.Y, 0, 0)
            };

            Canvas.SetZIndex(drawedNode, 4);

            mainWindow.MainCanvas.Children.Add(drawedNode);
        }

        /// <summary>
        /// Method for drawing <see cref="Edge"/> on <see cref="Canvas"/>.
        /// </summary>
        /// <param name="edge"></param>
        private void DrawSingleEdge(Edge edge)
        {
            Vector2 position1 = edge.Nodes.Item1.Position;
            Vector2 position2 = edge.Nodes.Item2.Position;

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

        /// <summary>
        /// Method for drawing highlighted <see cref="Edge"/> on <see cref="Canvas"/>.
        /// <see cref="HighlightNodeAndEdges(Point)"/>
        /// </summary>
        /// <param name="edge"></param>
        private void DrawHighlightedEdge(Edge edge)
        {
            Vector2 position1 = edge.Nodes.Item1.Position;
            Vector2 position2 = edge.Nodes.Item2.Position;

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

        /// <summary>
        /// Loop for drawing all <see cref="Nodes"/>.
        /// </summary>
        private void DrawNodes()
        {
            foreach (Node node in Nodes)
            {
                DrawSingleNode(node);
            }
        }

        /// <summary>
        /// Loop for drawing all <see cref="Edges"/>.
        /// </summary>
        private void DrawEdges()
        {
            foreach (Edge edge in Edges)
            {
                DrawSingleEdge(edge);
            }
        }

        /// <summary>
        /// Loop for drawing all highlighted <see cref="Edges"/>.
        /// </summary>
        private void DrawHighlightedEdges(Node node)
        {
            foreach (Edge edge in Edges)
            {
                if (edge.Nodes.Item1.ID == node.ID || edge.Nodes.Item2.ID == node.ID)
                {
                    DrawHighlightedEdge(edge);
                }
            }
        }

        /// <summary>
        /// Method with converts <see cref="adjacencyMatrix"/> pseudo matrix into string to export it to file.
        /// </summary>
        /// <returns>
        /// String with pseudo metrix ready to save in file.
        /// </returns>
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

        /// <summary>
        /// Method with converts string imported from file into <see cref="adjacencyMatrix"/> pseudo matrix variable.
        /// </summary>
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

        /// <summary>
        /// Miscellaneous method which replace Nodes with dog image :)
        /// </summary>
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

                Canvas.SetZIndex(doggie, 10);

                mainWindow.MainCanvas.Children.Add(doggie);
            }
        }
    }

    /// <summary>
    /// Class representing single node instance. It contains position, displacement and GlobalID
    /// which increments on every new object.
    /// </summary>
    class Node
    {
        private static int globalID = 0;
        /// <summary>
        /// Property representing <see cref="Node"/> position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Property representing <see cref="Node"/> displacement.
        /// </summary>
        public Vector2 Displacement;

        /// <summary>
        /// Property representing connections between <see cref="Node"/>s.
        /// </summary>
        public List<Node> Connections { get; set; } = new List<Node>();

        /// <summary>
        /// Property which holds specyfic <see cref="Node"/> ID number.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Constructor which initiate new <see cref="Node"/> with random position 
        /// in between <see cref="Canvas"/> boundaries.
        /// </summary>
        /// <param name="globalRandom"></param>
        /// <param name="mainWindow"></param>
        public Node(System.Random globalRandom, MainWindow mainWindow)
        {
            ID = globalID++;
            double shrinkedCanvasWidth = mainWindow.MainCanvas.ActualWidth - 20;
            double shrinkedCanvasHeight = mainWindow.MainCanvas.ActualHeight - 20;

            Position.X = globalRandom.NextFloatRange(0, (float)shrinkedCanvasWidth);
            Position.Y = globalRandom.NextFloatRange(0, (float)shrinkedCanvasHeight);
        }

        /// <summary>
        /// Method which resets back main Class ID to 0.
        /// </summary>
        public static void ClearID()
        {
            globalID = 0;
        }
    }

    /// <summary>
    /// Class representing single edge instance. It contains two references to Nodes between which a connection is created.
    /// </summary>
    class Edge
    {
        /// <summary>
        /// List of Corresponding <see cref="Node"/> pair.
        /// </summary>
        public Tuple<Node, Node> Nodes { get; }

        /// <summary>
        /// Constructor which creates tuplce containing corresponding <see cref="Node"/>s.
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        public Edge(Node firstNode, Node secondNode)
        {
            Nodes = new Tuple<Node, Node>(firstNode, secondNode);
        }
    }
}
