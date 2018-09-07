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

		public readonly List<RenderedPiece> Pieces = new List<RenderedPiece>();

		public RenderedPiece GrabbedPiece
		{ get; set; }

		public Vector2 GrabOffset
		{ get; set; }

		public ChessBoardGame(Canvas control) : base(control)
		{
			Scale = 0.5;

			Board = new Sprite(Canvas, "img/grayboard_1024.png", new Point(50, 50), Scale);
			Board.Control.Tag = "\"QUALITY\" CHESS BOARD";

			Sprites.Add(Board);

			SetUpPieces();
		}

		public override void Initialize(RoutedEventArgs e)
		{
			Board.Initialize();

			foreach (RenderedPiece rp in Pieces)
			{
				rp.Initialize();
			}

			base.Initialize(e);
		}

		public void SetUpPieces()
		{
			for (int i = 0; i < 8; i++)
			{
				Pieces.Add(new RenderedPiece(PieceType.Pawn, Side.White, i, 1, Board, Canvas));
				Pieces.Add(new RenderedPiece(PieceType.Pawn, Side.Black, i, 6, Board, Canvas));
			}

			Pieces.Add(new RenderedPiece(PieceType.Rook, Side.White, 0, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Knight, Side.White, 1, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Bishop, Side.White, 2, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Queen, Side.White, 3, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.King, Side.White, 4, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Bishop, Side.White, 5, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Knight, Side.White, 6, 0, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Rook, Side.White, 7, 0, Board, Canvas));

			Pieces.Add(new RenderedPiece(PieceType.Rook, Side.Black, 0, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Knight, Side.Black, 1, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Bishop, Side.Black, 2, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Queen, Side.Black, 3, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.King, Side.Black, 4, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Bishop, Side.Black, 5, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Knight, Side.Black, 6, 7, Board, Canvas));
			Pieces.Add(new RenderedPiece(PieceType.Rook, Side.Black, 7, 7, Board, Canvas));
		}

		public void GrabPiece(RenderedPiece piece, MouseButtonEventArgs e)
		{
			GrabbedPiece = piece;
			GrabOffset = e.GetPosition(piece.Sprite.Control);
			Panel.SetZIndex(GrabbedPiece.Sprite.Control, 1000);
		}

		public void DropPiece(Point boardPos)
		{
			if (GrabbedPiece == null)
			{
				return;
			}

			Vector2 posCenter = GrabbedPiece.RenderedPos + GrabbedPiece.Sprite.ActualSize / 2.0;
			(int col, int row) tile = getCoords(posCenter);
			GrabbedPiece.Piece.Column = tile.col;
			GrabbedPiece.Piece.Row = tile.row;
			GrabbedPiece.RenderedPos = new Vector2(tile.col / 8.0 * Board.ActualWidth, (7 - tile.row) / 8.0 * Board.ActualHeight) + Board.Position;

			Panel.SetZIndex(GrabbedPiece.Sprite.Control, 10);

			GrabbedPiece.Refresh();

			GrabbedPiece = null;
		}

		public override void Refresh(MouseEventArgs e)
		{
			if (GrabbedPiece != null)
			{
				GrabbedPiece.RenderedPos = e.GetPositionV() - GrabOffset;
				GrabbedPiece.Refresh();
			}

			base.Refresh(e);
		}

		private (int col, int row) getCoords(Vector2 pos)
		{
			Vector2 relPos = pos - Board.Position;

			// No idea why, but there's an off-by-one here.
			int tx = (int)(relPos.X / Board.ActualWidth * 8.0);
			int ty = 7 - (int)(relPos.Y / Board.ActualHeight * 8.0);

			return (tx, ty);
		}

		public override string GetDebugText(MouseEventArgs e)
		{
			List<string> lines = new List<string>();

			Vector2 posRel = e.GetPosition(Board.Control);
			lines.Add("Board coords: " + posRel.ToString());

			int tileX = Math.Min((int)(posRel.X / Board.ActualWidth * 8), 7);
			int tileY = 7 - Math.Min((int)(posRel.Y / Board.ActualHeight * 8), 7);

			lines.Add("Tile: " + Util.GetPosAlgebraic(tileY, tileX));

			if (GrabbedPiece != null)
			{
				lines.Add("Grabbed Piece: " + GrabbedPiece.Piece.ToString());

				Vector2 posCenter = GrabbedPiece.RenderedPos + GrabbedPiece.Sprite.ActualSize / 2.0;
				(int col, int row) tile = getCoords(posCenter);

				lines.Add("(at " + Util.GetPosAlgebraic(tile.row, tile.col) + ")");
			}

			string res = "";
			foreach (string l in lines)
			{
				res += l + "\n";
			}

			return res;
		}
	}
}
