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
		public Piece Piece => Move.Piece;

		public Move Move
		{ get; }

		public Tile From => Move.From;
		public Tile To => Move.To;

		public PieceMovedEventArgs(Move move)
		{
			Move = move;
		}
	}
}
