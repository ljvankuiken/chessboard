using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessBoard;

namespace ChessBoardWPFDisplay
{
	public class GhostPiece : RenderedPiece
	{
		public const double OPACITY = 0.6;

		public GhostPiece(Piece piece, Sprite board, Canvas canvas) : base(piece, board, canvas)
		{
			Refresh();
		}
		public GhostPiece(RenderedPiece clonedFrom, Canvas canvas)
		{
			Piece = clonedFrom.Piece;
			BoardSprite = clonedFrom.BoardSprite;

			//double x = (clonedFrom.Piece.Column / 8.0 * BoardSprite.ActualWidth) + BoardSprite.Position.X;
			//double y = ((7 - tile.Row) / 8.0 * BoardSprite.ActualHeight) + BoardSprite.Position.Y;
			RenderedPos = clonedFrom.RenderedPos;

			Sprite = new Sprite(canvas, GetImgPath(clonedFrom.Piece.Type, clonedFrom.Piece.Side), 
				RenderedPos, BoardSprite.Scale, OPACITY);
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
				Fill = new SolidColorBrush(Colors.ForestGreen),
				StrokeThickness = 2,
				Stroke = new SolidColorBrush(Colors.Black),
				Opacity = 0
			};
			DebugBoundingBox.SetPos(RenderedPos);
			Panel.SetZIndex(DebugBoundingBox, 9999);
			canvas.Children.Add(DebugBoundingBox);

			ShowDebug = clonedFrom.ShowDebug;
		}
	}
}
