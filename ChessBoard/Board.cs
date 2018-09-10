using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	/// <summary>
	/// Represents one of the two playable sides of chess. Used to define the side of pieces and turns.
	/// </summary>
	public enum Side
	{
		White,
		Black
	}

	/// <summary>
	/// Represents a game of chess.
	/// </summary>
    public class Board
    {
		/// <summary>
		/// Object to test validity of potential moves.
		/// </summary>
		public MovementValidator Validator
		{ get; }

		/// <summary>
		/// Side who's turn it is. Initialized to <see cref="Side.White"/>.
		/// </summary>
		public Side Turn
		{ get; private set; }

		/// <summary>
		/// List of all moves taken, by both sides.
		/// </summary>
		public List<Move> Moves
		{ get; } = new List<Move>();

		/// <summary>
		/// All pieces on board. Null items should not be allowed here.
		/// </summary>
		public List<Piece> Pieces
		{ get; } = new List<Piece>();

		public event PieceMovedEventHandler OnPieceMoved;

		public Piece this[int row, int col]
		{
			get => this[new Tile(row, col)];
			set => this[new Tile(row, col)] = value;
		}
		
		/// <summary>
		/// Accesses <see cref="Pieces"/> by location. Setting a location removes and replaces the <see cref="Piece"/> already there.
		/// </summary>
		/// <param name="tile">Location to access.</param>
		/// <returns>Piece at location, or null if no piece is found.</returns>
		public Piece this[Tile tile]
		{
			get
			{
				foreach (Piece p in Pieces)
				{
					if (p.Position == tile)
					{
						return p;
					}
				}

				return null;
			}
			internal set
			{
				// Remove replaced piece
				for (int i = Pieces.Count - 1; i >= 0; i--)
				{
					if (Pieces[i].Position == tile)
					{
						Pieces.RemoveAt(i);
					}
				}

				if (value != null)
				{
					// Add piece if not already present
					if (!Pieces.Contains(value))
					{
						Pieces.Add(value);
					}

					// Update position
					value.Position = tile;
				}
			}
		}

		public Board()
		{
			Validator = new MovementValidator(this);
			
			Reset();
		}

		/// <summary>
		/// Switches whose turn it is to the opposite side. Should be called after every move.
		/// </summary>
		public void SwitchTurn()
		{
			Turn = Turn.Opposite();
		}

		/// <summary>
		/// Resets the board to its initial status. Clears moves.
		/// </summary>
		public void Reset()
		{
			Moves.Clear();

			Pieces.Clear();

			Pieces.Add(new Piece(PieceType.Rook,	Side.White, new Tile(0, 0), this));
			Pieces.Add(new Piece(PieceType.Knight,	Side.White, new Tile(0, 1), this));
			Pieces.Add(new Piece(PieceType.Bishop,	Side.White, new Tile(0, 2), this));
			Pieces.Add(new Piece(PieceType.Queen,	Side.White, new Tile(0, 3), this));
			Pieces.Add(new Piece(PieceType.King,	Side.White, new Tile(0, 4), this));
			Pieces.Add(new Piece(PieceType.Bishop,	Side.White, new Tile(0, 5), this));
			Pieces.Add(new Piece(PieceType.Knight,	Side.White, new Tile(0, 6), this));
			Pieces.Add(new Piece(PieceType.Rook,	Side.White, new Tile(0, 7), this));

			Pieces.Add(new Piece(PieceType.Rook,	Side.Black, new Tile(7, 0), this));
			Pieces.Add(new Piece(PieceType.Knight,	Side.Black, new Tile(7, 1), this));
			Pieces.Add(new Piece(PieceType.Bishop,	Side.Black, new Tile(7, 2), this));
			Pieces.Add(new Piece(PieceType.Queen,	Side.Black, new Tile(7, 3), this));
			Pieces.Add(new Piece(PieceType.King,	Side.Black, new Tile(7, 4), this));
			Pieces.Add(new Piece(PieceType.Bishop,	Side.Black, new Tile(7, 5), this));
			Pieces.Add(new Piece(PieceType.Knight,	Side.Black, new Tile(7, 6), this));
			Pieces.Add(new Piece(PieceType.Rook,	Side.Black, new Tile(7, 7), this));

			for (int col = 0; col < 8; col++)
			{
				Pieces.Add(new Piece(PieceType.Pawn, Side.White, new Tile(1, col), this));
				Pieces.Add(new Piece(PieceType.Pawn, Side.Black, new Tile(6, col), this));
			}

			Turn = Side.White;
		}
		
		internal void RemoveAt(Tile tile)
		{
			for (int i = Pieces.Count - 1; i >= 0; i--)
			{
				if (Pieces[i].Position == tile)
				{
					Pieces.RemoveAt(i);
				}
			}
		}

		internal void Remove(Piece piece)
		{
			Pieces.Remove(piece);
		}

		internal void AfterPieceMoved(PieceMovedEventArgs e)
		{
			OnPieceMoved(e.Move, e);
		}
	}
}
