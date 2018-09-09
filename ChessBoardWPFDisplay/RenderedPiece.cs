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
		{ get; protected set; }

		public Vector2 RenderedPos
		{ get; set; }

		public Sprite Sprite
		{ get; protected set; }

		public Sprite BoardSprite
		{ get; protected set; }

		public Ellipse DebugCenterDot
		{ get; protected set; }

		public Rectangle DebugBoundingBox
		{ get; protected set; }

		public bool ShowDebug
		{ get; set; }

		protected RenderedPiece()
		{ }

		public RenderedPiece(Piece piece, Sprite board, Canvas canvas)
		{
			Piece = piece;
			BoardSprite = board;

			double x = (Piece.Position.Column / 8.0 * BoardSprite.ActualWidth) + BoardSprite.Position.X;
			double y = ((7 - Piece.Position.Row) / 8.0 * BoardSprite.ActualHeight) + BoardSprite.Position.Y;
			RenderedPos = new Vector2(x, y);
			
			Sprite = new Sprite(canvas, GetImgPath(Piece.Type, Piece.Side), RenderedPos, BoardSprite.Scale);
			Sprite.Control.Tag = Piece.ToString();
			Panel.SetZIndex(Sprite.Control, 10);

			DebugCenterDot = new Ellipse() {
				Width = 10,
				Height = 10,
				Fill = new SolidColorBrush(Colors.Red),
				Opacity = 0
			};
			Vector2 dotPos = RenderedPos + (Sprite.ActualSize / 2.0) - new Vector2(5);
			DebugCenterDot.SetPos(dotPos);
			Panel.SetZIndex(DebugCenterDot, 10000);
			canvas.Children.Add(DebugCenterDot);

			DebugBoundingBox = new Rectangle() {
				Width = Sprite.ActualWidth,
				Height = Sprite.ActualHeight,
				Fill = new SolidColorBrush(Colors.Goldenrod),
				StrokeThickness = 2,
				Stroke = new SolidColorBrush(Colors.Black),
				Opacity = 0
			};
			DebugBoundingBox.SetPos(RenderedPos);
			Panel.SetZIndex(DebugBoundingBox, 9999);
			canvas.Children.Add(DebugBoundingBox);
		}

		public void Disconnect(Canvas canvas)
		{
			canvas.Children.Remove(Sprite.Control);
			canvas.Children.Remove(DebugCenterDot);
			canvas.Children.Remove(DebugBoundingBox);
		}

		public static string GetImgPath(PieceType type, Side side)
		{
			return "img/qualitypieces/" + type.ToString().ToLower() + "_" + 
				side.ToString().ToLower() + ".png";
		}

		public void Initialize()
		{
			Sprite.Initialize();
		}

		public void Refresh()
		{
			Sprite.Position = RenderedPos;
			Sprite.Refresh();

			DebugCenterDot.Opacity = ShowDebug ? 1 : 0;
			Vector2 dotPos = RenderedPos + (Sprite.ActualSize / 2.0) - new Vector2(5);
			DebugCenterDot.SetPos(dotPos);

			DebugBoundingBox.Opacity = ShowDebug ? 0.3 : 0;
			DebugBoundingBox.SetPos(RenderedPos);
		}
    }
}
