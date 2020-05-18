using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CSgrapher
{
    /// <summary>
    /// Interaction logic for MatrixCreator.xaml
    /// </summary>
    public partial class MatrixCreator : Window
    {
        private readonly int nodeCount;
        /// <summary>
        /// Property which stores node connections in form of 2-dimensional <see cref="List"/> of <see cref="int"/>.
        /// </summary>
        public List<List<int>> AdjecencyMatrix { get; } = new List<List<int>>();

        /// <summary>
        /// Concstructor which initialize window and set <see cref="nodeCount"/> to value from param.
        /// </summary>
        /// <param name="nodeCount"></param>
        public MatrixCreator(int nodeCount)
        {
            InitializeComponent();
            this.nodeCount = nodeCount;

            CreateDescription();
        }

        /// <summary>
        /// Method for handling click on "OK" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDialogOk_Click(object sender, RoutedEventArgs e)
        {
            CreateMatrix();
            DialogResult = true;
        }

        /// <summary>
        /// Method which creates and display pseudo matrix with checkboxes,
        /// based on <see cref="nodeCount"/> in <see cref="MatrixCreator"/> window.
        /// </summary>
        private void CreateDescription()
        {
            for (int i = 0; i < nodeCount; i++)
            {
                Label rowId = new Label
                {
                    Width = 16,
                    Height = 16,
                    Padding = new Thickness(0, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = i
                };

                Row.Children.Add(rowId);

                Label columnID = new Label
                {
                    Width = 16,
                    Height = 16,
                    Padding = new Thickness(0, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = i
                };

                Column.Children.Add(columnID);

                WrapPanel singleRow = new WrapPanel();

                for (int j = 0; j <= i; j++)
                {
                    CheckBox checkBox = new CheckBox
                    {
                        Width = 16,
                        Height = 16
                    };

                    if (j == i)
                    {
                        checkBox.IsEnabled = false;
                    }

                    singleRow.Children.Add(checkBox);
                }

                MainPanel.Children.Add(singleRow);
            }
        }

        /// <summary>
        /// Method which parse data from checboxes to pseudo matrix in <see cref="AdjecencyMatrix"/>.
        /// </summary>
        private void CreateMatrix()
        {
            foreach (Panel row in MainPanel.Children)
            {
                List<int> matrixRow = new List<int>();

                foreach (CheckBox checkbox in row.Children)
                {
                    if (checkbox.IsChecked == true)
                    {
                        matrixRow.Add(1);
                    }
                    else
                    {
                        matrixRow.Add(0);
                    }
                }

                AdjecencyMatrix.Add(matrixRow);
            }
        }
    }
}
