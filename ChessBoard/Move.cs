using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	/// <summary>
	/// Represents a single move in chess.
	/// </summary>
	public class Move
	{
		/// <summary>
		/// <see cref="Piece"/> being moved.
		/// </summary>
		public Piece Piece
		{ get; }

		/// <summary>
		/// <see cref="Tile"/> piece moved from.
		/// </summary>
		public Tile From
		{ get; }

		/// <summary>
		/// <see cref="Tile"/> piece moved to.
		/// </summary>
		public Tile To
		{ get; }

		/// <summary>
		/// Whether the move is a capture or not. Includes en passant.
		/// </summary>
		public bool IsCapture
		{ get; }

		/// <summary>
		/// <see cref="Board"/> on which the move takes place.
		/// </summary>
		public Board Board
		{ get; }

		public Move(Piece moved, Tile to, Board board)
		{
			Board = board;
			Piece = moved;
			From = Piece.Position;
			To = to;
			IsCapture = Board[to] != null;

			// En passant capture identifying
			int forward = moved.Side == Side.White ? 1 : -1;
			Tile behind = new Tile(To.Row - forward, To.Column);
			if (moved.Type == PieceType.Pawn && Board[behind] != null)
			{
				IsCapture = true;
			}
		}

		/// <summary>
		/// Activates the move on the <see cref="Board"/>. Should only ever be called once.
		/// </summary>
		public virtual void DoMove()
		{
			// En passant capture
			Tile diff = To - From;
			if (Piece.Type == PieceType.Pawn && diff.Abs() == Tile.UnitRC && Board[To] == null)
			{
				Tile victim = From + diff.ColumnOnly;
				Board.RemoveAt(victim);
			}
			
			Board[To] = Piece;
			Piece.HasMoved = true;

			Board.AfterPieceMoved(new PieceMovedEventArgs(this));
		}
	}
}
