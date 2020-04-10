using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CSgrapher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        private Graph.Graph graph;
        private double zoomValue = 1;
        private static System.Windows.Threading.DispatcherTimer timer;
        private int sequentialCounter = 0;

        public MainWindow()
        {
            InitializeComponent();

            AppWindow = this;

            MainCanvas.LayoutTransform = new ScaleTransform(zoomValue, zoomValue);
        }

        private void ScrollViewer_Zoom_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _ = e.Delta > 0 ? zoomValue += 0.1 : zoomValue -= 0.1;
            }

            MainCanvas.LayoutTransform = new ScaleTransform(zoomValue, zoomValue);

            e.Handled = true;
        }

        private void Menu_TideUp_Click(object sender, RoutedEventArgs e)
        {
            sequentialCounter = 0;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(Sequentia_TideUp_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();

            e.Handled = true;
        }

        private void Sequentia_TideUp_Tick(object sender, EventArgs e)
        {
            sequentialCounter++;

            ForceCalculator.ForceCalculator forceCalculator = new ForceCalculator.ForceCalculator(AppWindow);
            forceCalculator.CalculateForces(graph);

            graph.DrawTidyGraph();

            sequentialProggresBar.Value = sequentialCounter;

            CommandManager.InvalidateRequerySuggested();
            if (sequentialCounter >= 120)
            {
                timer.Stop();
            }
        }

        private void Menu_NewRandom_Click(object sender, RoutedEventArgs e)
        {
            NodeCount nodeCountPopUp = new NodeCount();
            int nodeCount;

            if (nodeCountPopUp.ShowDialog() == true)
            {
                sequentialCounter = 0;
                nodeCount = Int32.Parse(nodeCountPopUp.Answer);
                graph = new Graph.Graph(AppWindow, nodeCount);
                graph.DrawGraph();
            }

            e.Handled = true;
        }

        private void Menu_New_Click(object sender, RoutedEventArgs e)
        {
            NodeCount nodeCountPopUp = new NodeCount();

            if (nodeCountPopUp.ShowDialog() == true)
            {
                int nodeCount = Int32.Parse(nodeCountPopUp.Answer);
                MatrixCreator matrixCreator = new MatrixCreator(nodeCount);

                if (matrixCreator.ShowDialog() == true)
                {
                    graph = new Graph.Graph(AppWindow, matrixCreator.AdjecencyMatrix);
                    graph.DrawGraph();
                }
            }

            e.Handled = true;
        }

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
                graph = new Graph.Graph(AppWindow, adjacencyMatrixString);
                graph.DrawGraph();
            }

            e.Handled = true;
        }

        private void Menu_DogUp_Click(object sender, RoutedEventArgs e)
        {
            graph.ConvertToDogs();

            e.Handled = true;
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (graph != null && graph.NodesCount > 0)
            {
                Point mousePosition = e.GetPosition(MainCanvas);
                graph.HighlightNodeAndEdges(mousePosition);
            }

            e.Handled = true;

        }
    }
}
