using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSgrapher
{
    /// <summary>
    /// Interaction logic for MatrixCreator.xaml
    /// </summary>
    public partial class MatrixCreator : Window
    {
        private int nodeCount;
        public MatrixCreator(int nodeCount)
        {
            InitializeComponent();
            this.nodeCount = nodeCount;

            CreateDescription();
        }

        private void ButtonDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CreateDescription()
        {
            for (int i = 1; i <= nodeCount; i++)
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

                for (int j = 1; j <= i; j++)
                {
                    CheckBox checkBox = new CheckBox();

                    checkBox.Width = 16;
                    checkBox.Height = 16;

                    if (j == i)
                    {
                        checkBox.IsEnabled = false;
                    }

                    singleRow.Children.Add(checkBox);
                }

                MainPanel.Children.Add(singleRow);
            }
        }
    }
}
