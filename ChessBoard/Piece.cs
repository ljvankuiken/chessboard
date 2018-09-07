using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	/// <summary>
	/// The particular type of piece on (or off of) the board.
	/// </summary>
	public enum PieceType
	{
		King = 0,
		Pawn,
		Knight,
		Bishop,
		Rook,
		Queen
	}

	/// <summary>
	/// Represents an individual piece on the board
	/// </summary>
	public class Piece
	{
		/// <summary>
		/// The zero-indexed column value for the piece's position. 0-7 are valid.
		/// </summary>
		public int Column
		{ get; set; }

		/// <summary>
		/// The zero-indexed row value for the piece's position. 0-7 are valid.
		/// Note that this differs by one from standard chess notation, which is one-indexed.
		/// </summary>
		public int Row
		{ get; set; }

		/// <summary>
		/// The type of this piece.
		/// </summary>
		public PieceType Type
		{ get; private set; }

		/// <summary>
		/// Which player controls this piece.
		/// </summary>
		public Side Side
		{ get; private set; }

		/// <summary>
		/// Returns the chess notation position for this piece (e.g., "e5").
		/// </summary>
		public string ChessPos => Util.GetPosAlgebraic(Row, Column);

		/// <summary>
		/// Returns whether this piece has moved.
		/// Meaningless if Type != PieceType.Pawn, but otherwise still usable.
		/// </summary>
		public bool HasPawnMoved => (Side == Side.White && Row != 1) || (Side == Side.Black && Row != 6);

		public Piece(PieceType type, Side side, int col, int row)
		{
			Column = col;
			Row = row;
			Type = type;
			Side = side;
		}

		public void Promote(PieceType promotedTo)
		{
			if (promotedTo == PieceType.King)
			{
				throw new ArgumentException("Pawns cannot be promoted to kings.");
			}

			if (Type == PieceType.Pawn)
			{
				if ((Side == Side.White && Row == 7) || (Side == Side.Black && Row == 0))
				{
					Type = promotedTo;
				}
			}
		}

		public override string ToString()
		{
			return Side.ToString() + " " + Type.ToString() + " at " + ChessPos;
		}
	}
}
