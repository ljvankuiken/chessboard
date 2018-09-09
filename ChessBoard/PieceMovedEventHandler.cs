using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public delegate void PieceMovedEventHandler(object sender, PieceMovedEventArgs e);

	public class PieceMovedEventArgs
	{
		public Piece Piece
		{ get; }

		public Move Move
		{ get; }

		public Tile From => Move.From;
		public Tile To => Move.To;

		public PieceMovedEventArgs(Piece piece, Move move)
		{
			Piece = piece;
			Move = move;
		}
	}
}
