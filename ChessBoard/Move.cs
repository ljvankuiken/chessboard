using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public enum NotationType
	{
		LaymanTemporary = -1,
		Algebraic = 0,
		PGN,
		English,
		Coordinate,
		ICCF
	}

	/// <summary>
	/// Represents a single move in chess.
	/// </summary>
	public class Move
	{
		/// <summary>
		/// <see cref="Piece"/> being moved.
		/// </summary>
		public Piece Piece
		{ get; }

		/// <summary>
		/// <see cref="Tile"/> piece moved from.
		/// </summary>
		public Tile From
		{ get; }

		/// <summary>
		/// <see cref="Tile"/> piece moved to.
		/// </summary>
		public Tile To
		{ get; }

		/// <summary>
		/// Whether the move is a capture or not. Includes en passant.
		/// </summary>
		public bool IsCapture
		{ get; }

		/// <summary>
		/// <see cref="Board"/> on which the move takes place.
		/// </summary>
		public Board Board
		{ get; }

		public static NotationType PreferredNotation
		{ get; set; }

		public string NotationAlgebraic
		{ get; protected set; }

		public string NotationEnglish
		{ get; protected set; }

		internal Guid ID;

		public Move(Piece moved, Tile to, Board board, bool skipCheck = false)
		{
			ID = Guid.NewGuid();

			Board = board;
			Piece = moved;
			From = Piece.Position;
			To = to;
			IsCapture = Board[to] != null;

			// En passant capture identifying
			int forward = moved.Side == Side.White ? 1 : -1;
			Tile behind = new Tile(To.Row - forward, To.Column);
			if (moved.Type == PieceType.Pawn && Board[behind] != null && To.Column != From.Column)
			{
				IsCapture = true;
			}

			// These two notations depend on the positions of other pieces, so must be generated 
			//   and cached before those pieces are moved to other locations.
			NotationAlgebraic = generateStringAlgebraic();
			NotationEnglish = generateStringEnglish();

			if (!skipCheck)
			{
				AppendCheckNotation();
			}
		}

		/// <summary>
		/// Activates the move on the <see cref="Board"/>. Should only ever be called once.
		/// </summary>
		public virtual void DoMove()
		{
			// En passant capture
			Tile diff = To - From;
			if (Piece.Type == PieceType.Pawn && diff.Abs() == Tile.UnitRC && Board[To] == null)
			{
				Tile victimPos = From + diff.ColumnOnly;
				Board.RemoveAt(victimPos);
			}
			
			Piece victim = Board[To];
			if (victim != null)
			{
				Board.Pieces.Remove(victim);
			}
			Piece.Position = To;

			Piece.HasMoved = true;

			Board.AfterPieceMoved(new PieceMovedEventArgs(this));
		}

		public override string ToString()
		{
			switch (PreferredNotation)
			{
			case NotationType.LaymanTemporary:
				// non-standard for layman readability
				return $"{Piece.Side} {Piece.Type}: {From.ToStringAlgebraic()}-{To.ToStringAlgebraic()}";
			case NotationType.Algebraic:
				return NotationAlgebraic;
			case NotationType.PGN:
				return ToStringPGN();
			case NotationType.English:
				return NotationEnglish;
			case NotationType.Coordinate:
				return ToStringCoordinate();
			case NotationType.ICCF:
				return ToStringICCF();
			default:
				throw new Exception("Invalid notation: " + PreferredNotation.ToString());
			}
		}

		protected internal virtual string generateStringAlgebraic()
		{
			string res = To.ToStringAlgebraic();

			if (IsCapture)
			{
				res = "x" + res;

				if (Piece.Type == PieceType.Pawn)
				{
					res = From.FileLetter() + res;
				}
			}

			if (Piece.Type != PieceType.Pawn)
			{
				IEnumerable<Piece> ambigOthers = Board.Pieces.Where(p => p.Type == Piece.Type && p.Side == Piece.Side);
				if (ambigOthers.Count() > 1)
				{
					if (ambigOthers.UniqueAmongAll(p => p.Position.Column))
					{
						res = From.FileLetter().ToString() + res;
					}
					else if (ambigOthers.UniqueAmongAll(p => p.Position.Row))
					{
						res = (From.Row + 1).ToString() + res;
					}
					else
					{
						res = From.ToStringAlgebraic() + res;
					}
				}

				res = Piece.Type.Abbreviation() + res;
			}

			return res;
		}

		protected internal virtual string generateStringEnglish()
		{
			string res = "";

			if (IsCapture)
			{
				Piece victim = Board[To];

				// en passant
				if (victim == null && Piece.Type == PieceType.Pawn)
				{
					res = Piece.Position.FileEnglishAbbrev() + "PxPe.p.";
				}
				else
				{
					res = Piece.Type.Abbreviation(true) + "x" + victim.Type.Abbreviation(true);

					// Regular capture
					if (Piece.Type == PieceType.Pawn)
					{
						IEnumerable<Piece> ambigOthers = Board.Pieces.Where(p => p.Type == PieceType.Pawn && p.Side == Piece.Side);
						if (ambigOthers.Count() > 1)
						{
							res = From.FileEnglishAbbrev() + res;
						}
					}
					else
					{
						IEnumerable<Piece> ambigPossibleVictims = Board.Pieces.Where(p => p.Type == victim.Type && p.Side == victim.Side);
						if (ambigPossibleVictims.Count() > 1)
						{
							res += "/" + To.ToStringEnglish(Piece.Side);
						}
					}
				}
			}
			else
			{
				res = Piece.Type.Abbreviation(true);

				IEnumerable<Piece> ambigOthers = Board.Pieces.Where(p => p != Piece && p.Type == Piece.Type && p.Side == Piece.Side);
				if (ambigOthers.Count() > 1 && Piece.Type != PieceType.Pawn)
				{
					res += "(" + From.ToStringEnglish(Piece.Side) + ")";
				}

				res += "-" + To.ToStringEnglish(Piece.Side);
			}

			return res;
		}

		public virtual void AppendCheckNotation()
		{
			if (Board.CheckIfMated(Piece.Side.Opposite()) == Piece.Side.GetVictoryStatus())
			{
				NotationAlgebraic += "#";
				NotationEnglish += "mate";
			}
			else if (Board.IsInCheck(Piece.Side.Opposite()))
			{
				NotationAlgebraic += "+";
				NotationEnglish += "ch";
			}
		}

		public virtual string ToStringPGN()
		{
			return NotationAlgebraic;
		}

		public virtual string ToStringCoordinate()
		{
			return From.ToStringCoordinate() + "-" + To.ToStringCoordinate();
		}

		public virtual string ToStringICCF()
		{
			return From.ToStringICCF() + To.ToStringICCF();
		}
	}
}
