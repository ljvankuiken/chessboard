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
		public ChessBoardGame ChessWrapper
		{ get; }

		public MainWindow()
		{
			InitializeComponent();
			ChessWrapper = new ChessBoardGame(GameCanvas);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ChessWrapper.Initialize(e);
		}

		// For some reason Canvas events only proc from elements within the Canvas, but not from the Canvas itself.
		private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				foreach (RenderedPiece p in ChessWrapper.Pieces)
				{
					if (p.Sprite.ActualBounds.Contains(e.GetPositionV()))
					{
						ChessWrapper.GrabPiece(p, e);
						break;
					}
				}
			}
			else if (e.ChangedButton == MouseButton.Right)
			{
				//MessageBox.Show("Right-clicked: " + e.OriginalSource.ToString());
			}
		}

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			ChessWrapper.Refresh(e);

			DebugTxt.Text = ChessWrapper.GetDebugText(e);
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			ChessWrapper.DropPiece(e.GetPosition(ChessWrapper.BoardSprite.Control));
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F3)
			{
				ChessWrapper.DebugMode = !ChessWrapper.DebugMode;
			}

			if (e.Key == Key.F8)
			{
				ChessWrapper.EnforceTurns = !ChessWrapper.EnforceTurns;
			}
		}
	}
}
