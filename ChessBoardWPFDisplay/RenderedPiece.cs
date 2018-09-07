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

		private readonly Ellipse _centerDot;
		private readonly Rectangle _boundingBox;

		public bool ShowDebug
		{ get; set; }

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
				Fill = new SolidColorBrush(Colors.Red),
				Opacity = 0
			};
			Vector2 dotPos = RenderedPos + (Sprite.ActualSize / 2.0) - new Vector2(5);
			_centerDot.SetPos(dotPos);
			Panel.SetZIndex(_centerDot, 10000);
			canvas.Children.Add(_centerDot);

			_boundingBox = new Rectangle() {
				Width = Sprite.ActualWidth,
				Height = Sprite.ActualHeight,
				Fill = new SolidColorBrush(Colors.Goldenrod),
				StrokeThickness = 2,
				Stroke = new SolidColorBrush(Colors.Black),
				Opacity = 0
			};
			_boundingBox.SetPos(RenderedPos);
			Panel.SetZIndex(_boundingBox, 9999);
			canvas.Children.Add(_boundingBox);
		}

		public void Initialize()
		{
			Sprite.Initialize();
		}

		public void Refresh()
		{
			Sprite.Position = RenderedPos;
			Sprite.Refresh();

			_centerDot.Opacity = ShowDebug ? 1 : 0;
			Vector2 dotPos = RenderedPos + (Sprite.ActualSize / 2.0) - new Vector2(5);
			_centerDot.SetPos(dotPos);

			_boundingBox.Opacity = ShowDebug ? 0.3 : 0;
			_boundingBox.SetPos(RenderedPos);
		}
    }
}
