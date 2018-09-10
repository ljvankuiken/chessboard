using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public class MovePromotion : Move
	{
		public class PieceMovedPromotionEventArgs : PieceMovedEventArgs
		{
			public new MovePromotion Move
			{ get; }

			public PieceMovedPromotionEventArgs(MovePromotion promotion) : base(promotion)
			{
				Move = promotion;
			}
		}

		public PieceType Promotion
		{ get; set; }

		public string NotationPGN
		{ get; private set; }

		public MovePromotion(Piece piece, Tile tileTo, Board board, 
			PieceType promotion = PieceType.Queen) : base(piece, tileTo, board, true)
		{
			Promotion = promotion;
			NotationPGN = NotationAlgebraic;
		}

		public override void DoMove()
		{
			base.DoMove();
			Piece.Promote(Promotion);
		}

		public override void AppendCheckNotation()
		{
			NotationAlgebraic += Promotion.Abbreviation();
			NotationPGN += "=" + Promotion.Abbreviation();
			NotationEnglish += "(" + Promotion.Abbreviation(true) + ")";

			base.AppendCheckNotation();

			if (Board.CheckIfMated(Piece.Side.Opposite()) == Piece.Side.GetVictoryStatus())
			{
				NotationPGN += "#";
			}
			else if (Board.IsInCheck(Piece.Side.Opposite()))
			{
				NotationPGN += "+";
			}
		}

		public override string ToStringPGN()
		{
			return NotationPGN;
		}

		public override string ToStringCoordinate()
		{
			return base.ToStringCoordinate() + "(" + Promotion.Abbreviation() + ")";
		}

		public override string ToStringICCF()
		{
			return base.ToStringICCF() + ((int)Promotion).ToString();
		}
	}
}
