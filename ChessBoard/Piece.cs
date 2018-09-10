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
		/// The zero-indexed column and row values for the piece's position. 0-7 are valid within this.
		/// </summary>
		public Tile Position
		{ get; internal set; }

		/// <summary>
		/// The type of this piece.
		/// </summary>
		public PieceType Type
		{ get; private set; }

		/// <summary>
		/// Which player controls this piece.
		/// </summary>
		public Side Side
		{ get; }

		/// <summary>
		/// Returns the chess notation position for this piece (e.g., "e5").
		/// </summary>
		public string ChessPos => Util.GetPosAlgebraic(Position);

		/// <summary>
		/// Indicates whether this piece has moved.
		/// Used by Pawns, the King, Queen, and Rook for special first movement.
		/// </summary>
		public bool HasMoved
		{ get; set; }

		/// <summary>
		/// Indicates whether a pawn has just moved double for its first move.
		/// Needed when checking if en passant capture is possible.
		/// Meaningless for any other piece.
		/// </summary>
		public bool PawnJustMovedDouble
		{ get; set; }

		public Piece(PieceType type, Side side, Tile tile, Board board)
		{
			Position = tile;
			Type = type;
			Side = side;

			board.OnPieceMoved += AfterPieceMoved;
		}

		/// <summary>
		/// Promotes pawn to a higher type. Does nothing for any other type.
		/// </summary>
		/// <param name="promotedTo">Type of piece to be promoted to.</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="promotedTo"/> is <see cref="PieceType.King"/>.</exception>
		public void Promote(PieceType promotedTo)
		{
			if (promotedTo == PieceType.King)
			{
				throw new ArgumentException("Pawns cannot be promoted to kings.");
			}

			if (Type == PieceType.Pawn)
			{
				if ((Side == Side.White && Position.Row == 7) || (Side == Side.Black && Position.Row == 0))
				{
					Type = promotedTo;
					HasMoved = false;
				}
			}
		}

		public override string ToString()
		{
			return Side.ToString() + " " + Type.ToString() + " at " + ChessPos;
		}

		internal void AfterPieceMoved(object sender, PieceMovedEventArgs e)
		{
			if (e.Piece == this)
			{
				if (Type == PieceType.Pawn && e.From.Column == Position.Column &&
					Math.Abs(e.From.Row - Position.Row) == 2)
				{
					PawnJustMovedDouble = true;
				}
			}
			else if (e.Piece.Side != Side)
			{
				PawnJustMovedDouble = false;
			}
		}

		public bool EqualsShallow(Piece other)
		{
			return Type == other.Type && Side == other.Side && Position == other.Position;
		}
	}
}
