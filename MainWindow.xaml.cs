using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CSgrapher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graph.Graph graph;
        private double zoomValue = 1;
        private static System.Windows.Threading.DispatcherTimer timer;
        private int sequentialCounter = 0;

        /// <summary>
        /// MainWindow.xaml constructor. 
        /// Initializing MainWindow component, setting up global MainWindow variable
        /// and setting up initial zoom value for ZoomLabel.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ZoomLabel.Content = $"{(int)(zoomValue * 100)}%";
        }

        /// <summary>
        /// Method for handling zooming canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_Zoom_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    zoomValue = zoomValue < 2 ? zoomValue += 0.1 : zoomValue;
                }
                else
                {
                    zoomValue = zoomValue > 0.2 ? zoomValue -= 0.1 : zoomValue;
                }

                MainCanvas.LayoutTransform = new ScaleTransform(zoomValue, zoomValue);

                ZoomLabel.Content = $"{(int)(zoomValue * 100)}%";
            }

            e.Handled = true;
        }

        /// <summary>
        /// Method for handling execution of dispatched arrangement alghoritm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Arrange_Click(object sender, RoutedEventArgs e)
        {
            sequentialCounter = 0;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(Sequentia_Arrange_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();

            e.Handled = true;
        }

        /// <summary>
        /// Method for dispatcher <see cref="EventHandler"/> which executes
        /// force calculator for given <see cref="graph"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sequentia_Arrange_Tick(object sender, EventArgs e)
        {
            sequentialCounter++;

            ForceCalculator.ForceCalculator forceCalculator = new ForceCalculator.ForceCalculator(MainCanvas);
            forceCalculator.CalculateForces(graph);

            graph.DrawGraph();
            ResetStats();

            sequentialProggresBar.Value = sequentialCounter;

            if (sequentialCounter >= 120)
            {
                timer.Stop();
            }
        }

        /// <summary>
        /// Method for creating new random <see cref="graph"/> instance with
        /// given number of nodes in <see cref="NodeCount"/> window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_NewRandom_Click(object sender, RoutedEventArgs e)
        {
            NodeCount nodeCountPopUp = new NodeCount();
            int nodeCount;

            if (nodeCountPopUp.ShowDialog() == true)
            {
                sequentialCounter = 0;
                nodeCount = Int32.Parse(nodeCountPopUp.Answer);
                graph = new Graph.Graph(MainCanvas, nodeCount);
                graph.DrawGraph();
                ResetStats();
                EnableMenu();
            }

            e.Handled = true;
        }

        /// <summary>
        /// Method for creating new <see cref="graph"/> instance with
        /// given number of <see cref="Graph.Node"/> in <see cref="NodeCount"/> window
        /// and user-added <see cref="Graph.Edge"/>s.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_New_Click(object sender, RoutedEventArgs e)
        {
            NodeCount nodeCountPopUp = new NodeCount();

            if (nodeCountPopUp.ShowDialog() == true)
            {
                int nodeCount = Int32.Parse(nodeCountPopUp.Answer);
                MatrixCreator matrixCreator = new MatrixCreator(nodeCount);

                if (matrixCreator.ShowDialog() == true)
                {
                    graph = new Graph.Graph(MainCanvas, matrixCreator.AdjecencyMatrix);
                    graph.DrawGraph();
                    ResetStats();
                    EnableMenu();
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// Method handling graph to file saving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Graph file (*.graph)|*.graph",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, graph.AdjecencyMatrixString);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Method handling opening graph from file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Graph file (*.graph)|*.graph",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string adjacencyMatrixString = File.ReadAllText(openFileDialog.FileName);
                graph = new Graph.Graph(MainCanvas, adjacencyMatrixString);
                graph.DrawGraph();

                ResetStats();
            }

            e.Handled = true;
        }

        /// <summary>
        /// Method which converts <see cref="Graph.Node"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_DogUp_Click(object sender, RoutedEventArgs e)
        {
            graph.ConvertToDogs();

            e.Handled = true;
        }

        /// <summary>
        /// Method handling highlighting <see cref="Graph.Node"/> and corresponding
        /// <see cref="Graph.Edge"/> on <see cref="MainCanvas"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCanvas_MouseUp_HighlighNode(object sender, MouseButtonEventArgs e)
        {
            if (graph != null && graph.NodesCount > 0)
            {
                Point mousePosition = e.GetPosition(MainCanvas);
                graph.HighlightNodeAndEdges(mousePosition);
            }

            e.Handled = true;

        }

        /// <summary>
        /// Method which enables menu items when new graph is drawed on <see cref="MainCanvas"/>
        /// </summary>
        private void EnableMenu()
        {
            MenuDogUp.IsEnabled = true;
            MenuSave.IsEnabled = true;
            MenuTideUp.IsEnabled = true;
        }

        private void ResetStats()
        {
            sequentialProggresBar.Value = 0;
            EdgesCount.Content = graph.Edges.Count;
        }
    }
}
