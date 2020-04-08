using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CSgrapher
{
	public partial class NodeCount : Window
	{
		public NodeCount()
		{
			InitializeComponent();
			TextAnswer.Text = "5";
		}

		private void ButtonDialogOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			TextAnswer.SelectAll();
			TextAnswer.Focus();
		}

		public string Answer
		{
			get { return TextAnswer.Text; }
		}
	}
}