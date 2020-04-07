using System;
using System.Threading;
using System.Timers;
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
        public static MainWindow AppWindow;
        private Graph.Graph graph;
        private double zoomValue = 1;
        private static System.Windows.Threading.DispatcherTimer timer;
        private int sequentialCounter = 0;

        public MainWindow()
        {
            InitializeComponent();

            AppWindow = this;

            ScaleTransform scale = new ScaleTransform(zoomValue, zoomValue);
            MainCanvas.LayoutTransform = scale;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            graph.ConvertToDogs();
        }

        private void MainCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    zoomValue += 0.1;
                }
                else
                {
                    zoomValue -= 0.1;
                }
            }

            ScaleTransform scale = new ScaleTransform(zoomValue, zoomValue);
            MainCanvas.LayoutTransform = scale;
            e.Handled = true;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            sequentialCounter = 0;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            sequentialCounter++;
            ForceCalculator.ForceCalculator forceCalculator = new ForceCalculator.ForceCalculator(AppWindow);
            forceCalculator.CalculateForces(graph);
            graph.DrawTidyGraph();
            sequentialStatus.Value = sequentialCounter;
            CommandManager.InvalidateRequerySuggested();
            if(sequentialCounter >= 120)
            {
                timer.Stop();
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            NodeCount nodeCountPopUp = new NodeCount();
            int nodeCount;

            if (nodeCountPopUp.ShowDialog() == true)
            {
                sequentialStatus.Value = 0;
                sequentialCounter = 0;
                nodeCount = Int32.Parse(nodeCountPopUp.Answer);
                graph = new Graph.Graph(AppWindow, nodeCount);
                graph.DrawGraph();
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            NodeCount nodeCountPopUp = new NodeCount();
            int nodeCount = 10;

            if (nodeCountPopUp.ShowDialog() == true)
            {
                nodeCount = Int32.Parse(nodeCountPopUp.Answer);
                MatrixCreator matrixCreator = new MatrixCreator(nodeCount);

                if (matrixCreator.ShowDialog() == true)
                {
                    graph = new Graph.Graph(AppWindow, nodeCount);
                    graph.DrawGraph();

                }
            }
        }
    }
}
