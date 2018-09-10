using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public class MoveCastle : Move
	{
		public class PieceMovedCastleEventArgs : PieceMovedEventArgs
		{
			public new MoveCastle Move
			{ get; }

			public PieceMovedCastleEventArgs(MoveCastle castle) : base(castle)
			{
				Move = castle;
			}
		}

		public Piece Rook
		{ get; }

		public Tile RookTo
		{ get; }

		public bool IsKingsSide
		{ get; }

		public MoveCastle(Piece king, Piece rook, Tile kingTo, Board board) : base(king, kingTo, board, true)
		{
			Rook = rook;

			IsKingsSide = king.Position.Column < rook.Position.Column;

			RookTo = new Tile(king.Position.Row, kingTo.Column + (IsKingsSide ? -1 : 1));

			NotationAlgebraic = ToString();
			NotationEnglish = ToStringPGN();
		}

		public override void DoMove()
		{
			Piece.Position = To;
			Rook.Position = RookTo;

			Piece.HasMoved = true;
			Rook.HasMoved = true;

			Board.AfterPieceMoved(new PieceMovedCastleEventArgs(this));
		}

		public override string ToString()
		{
			if (Rook.Position.Column > Piece.Position.Column)
			{
				return "0-0";
			}
			else
			{
				return "0-0-0";
			}
		}

		public override string ToStringPGN()
		{
			return ToString().Replace('0', 'O');
		}
	}
}
