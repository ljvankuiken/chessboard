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

namespace ChessBoardWPFDisplay
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public readonly ChessBoardGame GameWrapper;

		public MainWindow()
		{
			InitializeComponent();
			GameWrapper = new ChessBoardGame(GameCanvas);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			GameWrapper.Initialize(e);
		}

		// For some reason this only procs when an object within the canvas is clicked, not the canvas itself.
		private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.OriginalSource is FrameworkElement)
			{
				FrameworkElement el = e.OriginalSource as FrameworkElement;
				MessageBox.Show("You clicked a thing: " + el.Tag.ToString());
			}
		}
	}
}
