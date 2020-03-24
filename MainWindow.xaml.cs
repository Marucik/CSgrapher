using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Graph;

namespace CSgrapher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        private Graph.Graph graph;
        public MainWindow()
        {
            InitializeComponent();

            AppWindow = this;
            graph = new Graph.Graph(AppWindow);

            int[] connections = { 1, 2, 3 };

            graph.AddNode(connections);

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int[] connections = { 1, 2, 3 };

            graph.AddNode(connections);

            graph.DrawNodes();
        }
    }
}
