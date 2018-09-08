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
		public readonly Piece[,] Layout = new Piece[8, 8];

		public Side Turn
		{ get; private set; }

		public Board()
		{

		}

		public void Reset()
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; i < 8; i++)
				{
					Layout[i, j] = null;
				}
			}

			Layout[0, 0] = new Piece(PieceType.Rook,	Side.White, new Tile(0, 0));
			Layout[0, 1] = new Piece(PieceType.Knight,	Side.White, new Tile(0, 1));
			Layout[0, 2] = new Piece(PieceType.Bishop,	Side.White, new Tile(0, 2));
			Layout[0, 3] = new Piece(PieceType.Queen,	Side.White, new Tile(0, 3));
			Layout[0, 4] = new Piece(PieceType.King,	Side.White, new Tile(0, 4));
			Layout[0, 5] = new Piece(PieceType.Bishop,	Side.White, new Tile(0, 5));
			Layout[0, 6] = new Piece(PieceType.Knight,	Side.White, new Tile(0, 6));
			Layout[0, 7] = new Piece(PieceType.Rook,	Side.White, new Tile(0, 7));

			Layout[7, 0] = new Piece(PieceType.Rook,	Side.Black, new Tile(7, 0));
			Layout[7, 1] = new Piece(PieceType.Knight,	Side.Black, new Tile(7, 1));
			Layout[7, 2] = new Piece(PieceType.Bishop,	Side.Black, new Tile(7, 2));
			Layout[7, 3] = new Piece(PieceType.Queen,	Side.Black, new Tile(7, 3));
			Layout[7, 4] = new Piece(PieceType.King,	Side.Black, new Tile(7, 4));
			Layout[7, 5] = new Piece(PieceType.Bishop,	Side.Black, new Tile(7, 5));
			Layout[7, 6] = new Piece(PieceType.Knight,	Side.Black, new Tile(7, 6));
			Layout[7, 7] = new Piece(PieceType.Rook,	Side.Black, new Tile(7, 7));

			for (int col = 0; col < 8; col++)
			{
				Layout[1, col] = new Piece(PieceType.Pawn, Side.White, new Tile(1, col));
				Layout[6, col] = new Piece(PieceType.Pawn, Side.Black, new Tile(6, col));
			}
		}
    }
}
