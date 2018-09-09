using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public class Move
	{
		public Piece Piece
		{ get; }

		public Tile From
		{ get; }

		public Tile To
		{ get; }

		public bool IsCapture
		{ get; }

		public Board Board
		{ get; }

		public Move(Piece moved, Tile to, Board board)
		{
			Board = board;
			Piece = moved;
			From = Piece.Position;
			To = to;
			IsCapture = Board[to] != null;
		}

		public void DoMove()
		{
			// En passant checking
			Tile diff = To - From;
			if (Piece.Type == PieceType.Pawn && diff.Abs() == Tile.UnitRC && Board[To] == null)
			{
				Tile victim = From + diff.ColumnOnly;
				Board[victim] = null;
			}

			Board[Piece.Position] = null;
			Board[To] = Piece;
			Piece.Position = To;
			Piece.HasMoved = true;

			foreach (Piece p in Board.Layout)
			{
				p?.AfterPieceMoved(Piece, From);
			}

			Board.AfterPieceMoved(this);
		}
	}
}
