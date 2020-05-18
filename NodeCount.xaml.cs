using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CSgrapher
{
	/// <summary>
	/// Interaction logic for NodeCount.xaml
	/// </summary>
	public partial class NodeCount : Window
	{
		/// <summary>
		/// Property which stores given number of <see cref="Graph.Node"/>.
		/// </summary>
		public string Answer
		{
			get { return TextAnswer.Text; }
		}

		/// <summary>
		/// Concstructor which initialize window and set <see cref="Answer"/> to initial value of 10.
		/// </summary>
		public NodeCount()
		{
			InitializeComponent();
			TextAnswer.Text = "10";
		}

		/// <summary>
		/// Method for handling click on "OK" button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonDialogOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		/// <summary>
		/// Method which checks if user typed number in <see cref="TextAnswer"/>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		/// <summary>
		/// Method which focuses <see cref="TextAnswer"/> when <see cref="Window"/> is rendered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_ContentRendered(object sender, EventArgs e)
		{
			TextAnswer.SelectAll();
			TextAnswer.Focus();
		}
	}
}