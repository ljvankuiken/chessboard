using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessBoard;

namespace ChessBoardWPFDisplay
{
    public class RenderedPiece
    {
		public Piece Piece
		{ get; private set; }

		public Vector2 RenderedPos
		{ get; set; }

		public Sprite Sprite
		{ get; private set; }

		public Sprite BoardSprite
		{ get; private set; }

		private Ellipse _centerDot;

		public RenderedPiece(PieceType type, Side side, int col, int row, Sprite board, Canvas canvas)
		{
			Piece = new Piece(type, side, col, row);
			BoardSprite = board;
			
			double y = ((7 - row) / 8.0 * BoardSprite.ActualHeight) + BoardSprite.Position.Y;
			double x = (col / 8.0 * BoardSprite.ActualWidth) + BoardSprite.Position.X;
			RenderedPos = new Vector2(x, y);

			string path = "img/qualitypieces/" + type.ToString().ToLower() + "_" + side.ToString().ToLower() + ".png";
			Sprite = new Sprite(canvas, path, RenderedPos, BoardSprite.Scale);
			Sprite.Control.Tag = Piece.ToString();
			Panel.SetZIndex(Sprite.Control, 10);

			_centerDot = new Ellipse() {
				Width = 10,
				Height = 10,
				Fill = new SolidColorBrush(Colors.Red)
			};
			Vector2 dotPos = RenderedPos + (Sprite.ActualSize / 2.0) - new Vector2(5);
			Canvas.SetLeft(_centerDot, dotPos.X);
			Canvas.SetTop(_centerDot, dotPos.Y);
			Panel.SetZIndex(_centerDot, 10000);
			canvas.Children.Add(_centerDot);
		}

		public void Initialize()
		{
			Sprite.Initialize();
		}

		public void Refresh()
		{
			Sprite.Position = RenderedPos;
			Sprite.Refresh();

			Vector2 dotPos = RenderedPos + (Sprite.ActualSize / 2.0) - new Vector2(5);
			Canvas.SetLeft(_centerDot, dotPos.X);
			Canvas.SetTop(_centerDot, dotPos.Y);
		}
    }
}
