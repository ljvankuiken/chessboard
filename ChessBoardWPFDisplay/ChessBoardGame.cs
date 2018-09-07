using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChessBoard;

namespace ChessBoardWPFDisplay
{
    public class ChessBoardGame : CanvasGame
    {
		public Sprite Board
		{ get; private set; }

		public double Scale
		{ get; private set; }

		public ChessBoardGame(Canvas control) : base(control)
		{
			Scale = 0.5;

			Board = new Sprite(Canvas, "img/shittyboard_1024.png", new Vector2(50), Scale);
			Board.Control.Tag = "SHITTY CHESS BOARD";

			Sprites.Add(Board);
		}

		public override void Initialize(RoutedEventArgs e)
		{
			Board.Initialize();

			base.Initialize(e);
		}

		public override string GetDebugText(MouseEventArgs e)
		{
			string res = "";

			Point posRel = e.GetPosition(Board.Control);
			res += "Board coords: " + posRel.ToString() + "\n";

			int tileX = Math.Min((int)(posRel.X / Board.ActualWidth * 8), 7);
			int tileY = 7 - Math.Min((int)(posRel.Y / Board.ActualHeight * 8), 7);

			res += "Tile: " + Util.GetPosAlgebraic(tileY, tileX);

			return res;
		}
	}
}
