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

		public MovePromotion(Piece piece, Tile tileTo, Board board, 
			PieceType promotion = PieceType.Queen) : base(piece, tileTo, board, true)
		{
			Promotion = promotion;
		}

		public override void DoMove()
		{
			base.DoMove();
			Piece.Promote(Promotion);
		}
	}
}
