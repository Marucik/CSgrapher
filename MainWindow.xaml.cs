using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ForceCalculator;

namespace CSgrapher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        private Graph.Graph graph;
        private double zoomValue = 0.8;

        public MainWindow()
        {
            InitializeComponent();

            AppWindow = this;

            ScaleTransform scale = new ScaleTransform(zoomValue, zoomValue);
            MainCanvas.LayoutTransform = scale;
        }

        private void MainCanvas_MouseDown(object sender, RoutedEventArgs e)
        {
            ForceCalculator.ForceCalculator forceCalculator = new ForceCalculator.ForceCalculator(AppWindow);
            forceCalculator.CalculateForces(graph);

            graph.DrawTidyGraph();
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
            ForceCalculator.ForceCalculator forceCalculator = new ForceCalculator.ForceCalculator(AppWindow);
            forceCalculator.CalculateForces(graph);

            graph.DrawTidyGraph();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            graph = new Graph.Graph(AppWindow);
            graph.DrawGraph();
        }
    }
}
