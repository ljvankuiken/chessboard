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
using System.Windows.Shapes;
using ChessBoard;

namespace ChessBoardWPFDisplay
{
	/// <summary>
	/// Interaction logic for PromotionsWindow.xaml
	/// </summary>
	public partial class PromotionsWindow : Window
	{
		public PieceType Result
		{ get; private set; }

		public PromotionsWindow()
		{
			InitializeComponent();

			Result = PieceType.Queen;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			QueenBtn.Focus();
		}

		private void QueenBtn_Click(object sender, RoutedEventArgs e)
		{
			Result = PieceType.Queen;
			DialogResult = true;
			Close();
		}

		private void RookBtn_Click(object sender, RoutedEventArgs e)
		{
			Result = PieceType.Rook;
			DialogResult = true;
			Close();
		}

		private void KnightBtn_Click(object sender, RoutedEventArgs e)
		{
			Result = PieceType.Knight;
			DialogResult = true;
			Close();
		}

		private void BishopBtn_Click(object sender, RoutedEventArgs e)
		{
			Result = PieceType.Bishop;
			DialogResult = true;
			Close();
		}
	}
}
