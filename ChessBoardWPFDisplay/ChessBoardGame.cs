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

			Board = new Sprite(Canvas, "img/shittyboard_1024.png", new Vector2(50), Scale);
			Board.Control.Tag = "SHITTY CHESS BOARD";

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
			GrabOffset = e.GetPosition(piece.Sprite.Control).ToVector2();
		}

		public void DropPiece(Vector2 boardPos)
		{
			GrabbedPiece = null;
		}

		public override void Refresh(MouseEventArgs e)
		{
			if (GrabbedPiece != null)
			{
				GrabbedPiece.RenderedPos = e.GetPosition(null).ToVector2() - GrabOffset;
				GrabbedPiece.Refresh();
			}

			base.Refresh(e);
		}

		public override string GetDebugText(MouseEventArgs e)
		{
			List<string> lines = new List<string>();

			Point posRel = e.GetPosition(Board.Control);
			lines.Add("Board coords: " + posRel.ToString());

			int tileX = Math.Min((int)(posRel.X / Board.ActualWidth * 8), 7);
			int tileY = 7 - Math.Min((int)(posRel.Y / Board.ActualHeight * 8), 7);

			lines.Add("Tile: " + Util.GetPosAlgebraic(tileY, tileX));

			if (GrabbedPiece != null)
			{
				lines.Add("Grabbed Piece: " + GrabbedPiece.Piece.ToString());
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
