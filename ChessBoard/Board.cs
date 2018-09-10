using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public enum Side
	{
		White,
		Black
	}

    public class Board
    {
		[Obsolete]
		public Piece[,] Layout
		{ get; } = new Piece[8, 8];

		public MovementValidator Validator
		{ get; }

		public Side Turn
		{ get; private set; }

		public List<Move> Moves
		{ get; } = new List<Move>();

		/// <summary>
		/// Condensed list of all pieces on board. Attempting to modify pieces from here does nothing.
		/// </summary>
		public List<Piece> Pieces
		{ get; } = new List<Piece>();
		//{
		//	get
		//	{
		//		List<Piece> res = new List<Piece>();
		//		foreach (Piece p in Layout)
		//		{
		//			if (p != null)
		//			{
		//				res.Add(p);
		//			}
		//		}
		//
		//		return res;
		//	}
		//}

		public event PieceMovedEventHandler OnPieceMoved;

		public Piece this[int row, int col]
		{
			//get => Layout[row, col];
			//set => Layout[row, col] = value;
			get => this[new Tile(row, col)];
			set => this[new Tile(row, col)] = value;
		}

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
			set
			{
				for (int i = Pieces.Count - 1; i >= 0; i--)
				{
					if (Pieces[i].Position == tile)
					{
						Pieces.RemoveAt(i);
					}
				}

				if (value != null)
				{
					if (!Pieces.Contains(value))
					{
						Pieces.Add(value);
					}
					value.Position = tile;
				}
			}
		}

		public Board()
		{
			Validator = new MovementValidator(this);
			
			Reset();
		}

		public void SwitchTurn()
		{
			Turn = Turn.Opposite();
		}

		public void Reset()
		{
			Moves.Clear();

			Pieces.Clear();
			//for (int i = 0; i < 8; i++)
			//{
			//	for (int j = 0; i < 8; i++)
			//	{
			//		Layout[i, j] = null;
			//	}
			//}

			//Layout[0, 0] = new Piece(PieceType.Rook,	Side.White, new Tile(0, 0), this);
			//Layout[0, 1] = new Piece(PieceType.Knight,	Side.White, new Tile(0, 1), this);
			//Layout[0, 2] = new Piece(PieceType.Bishop,	Side.White, new Tile(0, 2), this);
			//Layout[0, 3] = new Piece(PieceType.Queen,	Side.White, new Tile(0, 3), this);
			//Layout[0, 4] = new Piece(PieceType.King,	Side.White, new Tile(0, 4), this);
			//Layout[0, 5] = new Piece(PieceType.Bishop,	Side.White, new Tile(0, 5), this);
			//Layout[0, 6] = new Piece(PieceType.Knight,	Side.White, new Tile(0, 6), this);
			//Layout[0, 7] = new Piece(PieceType.Rook,	Side.White, new Tile(0, 7), this);
			//
			//Layout[7, 0] = new Piece(PieceType.Rook,	Side.Black, new Tile(7, 0), this);
			//Layout[7, 1] = new Piece(PieceType.Knight,	Side.Black, new Tile(7, 1), this);
			//Layout[7, 2] = new Piece(PieceType.Bishop,	Side.Black, new Tile(7, 2), this);
			//Layout[7, 3] = new Piece(PieceType.Queen,	Side.Black, new Tile(7, 3), this);
			//Layout[7, 4] = new Piece(PieceType.King,	Side.Black, new Tile(7, 4), this);
			//Layout[7, 5] = new Piece(PieceType.Bishop,	Side.Black, new Tile(7, 5), this);
			//Layout[7, 6] = new Piece(PieceType.Knight,	Side.Black, new Tile(7, 6), this);
			//Layout[7, 7] = new Piece(PieceType.Rook,	Side.Black, new Tile(7, 7), this);

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
				//Layout[1, col] = new Piece(PieceType.Pawn, Side.White, new Tile(1, col), this);
				//Layout[6, col] = new Piece(PieceType.Pawn, Side.Black, new Tile(6, col), this);
				Pieces.Add(new Piece(PieceType.Pawn, Side.White, new Tile(1, col), this));
				Pieces.Add(new Piece(PieceType.Pawn, Side.Black, new Tile(6, col), this));
			}

			Turn = Side.White;
		}

		internal void AfterPieceMoved(PieceMovedEventArgs e)
		{
			OnPieceMoved(e.Move, e);
		}
	}
}
