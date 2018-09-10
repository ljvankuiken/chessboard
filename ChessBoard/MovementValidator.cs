using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	/// <summary>
	/// Used to validate moves as legal or not.
	/// </summary>
	public class MovementValidator
	{
		/// <summary>
		/// Board on which to check moves.
		/// </summary>
		public Board Board
		{ get; }

		public bool BypassingCheck
		{ get; private set; }

		/// <summary>
		/// A <see cref="Dictionary{TKey, TValue}"/> of invalid <see cref="Tile"/>s and the reasons for which they are invalid.
		/// </summary>
		public Dictionary<Tile, string> InvalidErrors
		{ get; } = new Dictionary<Tile, string>();

		private List<Move> _validCache;
		private Tile _cachedPosition;
		private PieceType _cachedType;

		public MovementValidator(Board board)
		{
			Board = board;
		}

		/// <summary>
		/// Tests whether a <see cref="Piece"/> can legally move to a given <see cref="Tile"/>.
		/// </summary>
		/// <param name="piece">The piece tested.</param>
		/// <param name="target">The target location.</param>
		/// <returns><see cref="true"/> if the move is legal, <see cref="false"/> otherwise.</returns>
		public bool IsMovementValid(Piece piece, Tile target)
		{
			List<Move> valid = GetValidLocations(piece);

			return valid == null || valid.Exists(m => m.To == target);
		}

		/// <summary>
		/// Tests whether a <see cref="Piece"/> can legally move to a given <see cref="Tile"/>, and includes
		/// the <see cref="Move"/> needed to do so.
		/// </summary>
		/// <param name="piece">The piece tested.</param>
		/// <param name="target">The target location.</param>
		/// <param name="validated">
		/// The outputted <see cref="Move"/> needed to move the <see cref="piece"/> to the <see cref="target"/> tile,
		/// or null if the move is illegal.
		/// </param>
		/// <returns><see cref="true"/> if the move is legal, <see cref="false"/> otherwise.</returns>
		public bool IsMovementValid(Piece piece, Tile target, out Move validated)
		{
			List<Move> valid = GetValidLocations(piece);

			foreach (Move m in valid)
			{
				if (m.To == target)
				{
					validated = m;
					return true;
				}
			}

			validated = null;
			return false;
		}

		public List<Move> GetAllLegalMoves(Side side)
		{
			List<Move> res = new List<Move>();

			foreach (Piece p in Board.Pieces)
			{
				if (p.Side == side)
				{
					res.AddRange(GetValidLocations(p));
				}
			}

			return res;
		}

		/// <summary>
		/// Tests which moves a <see cref="Piece"/> can legally make, and returns a list of all legal ones.
		/// Caches results to prevent lag. Cache is reset if <see cref="Piece"/> differs in 
		/// <see cref="Side"/> or <see cref="PieceType"/>.
		/// </summary>
		/// <param name="piece">The piece tested.</param>
		/// <returns>
		/// A list of all legal moves <paramref name="piece"/> can make, or null if no restrictions are made.
		/// </returns>
		public List<Move> GetValidLocations(Piece piece, bool bypassCheck = false)
		{
			BypassingCheck = bypassCheck;

			if (_validCache != null && piece.Type == _cachedType && piece.Position == _cachedPosition && !BypassingCheck && false)
			{
				return _validCache;
			}
			else if (!BypassingCheck)
			{
				ResetCache();
				_cachedPosition = piece.Position;
				_cachedType = piece.Type;
			}


			List<Move> res = null;
			switch (piece.Type)
			{
			case PieceType.King:
				res = getValidKingLocations(piece);
				break;
			case PieceType.Pawn:
				res = getValidPawnLocations(piece);
				break;
			case PieceType.Knight:
				res = getValidKnightLocations(piece);
				break;
			case PieceType.Bishop:
				res = getValidBishopLocations(piece);
				break;
			case PieceType.Rook:
				res = getValidRookLocations(piece);
				break;
			case PieceType.Queen:
				res = getValidQueenLocations(piece);
				break;
			default:
				throw new ArgumentException("Piece must be from chess.", nameof(piece));
			}

			if (!BypassingCheck)
			{
				for (int i = res.Count - 1; i >= 0; i--)
				{
					Move m = res[i];
			
					if (WouldLeaveInCheck(m, piece.Side))
					{
						res.RemoveAt(i);
						addError(m.To, "STILL IN CHECK");
					}
				}
			}

			_validCache = res;

			return res;
		}

		public List<Tile> GetThreatenedTiles(Piece piece)
		{
			List<Tile> res = new List<Tile>();

			switch (piece.Type)
			{
			case PieceType.King:
				res = getThreatenedKingLocations(piece);
				break;
			case PieceType.Pawn:
				res = getThreatenedPawnLocations(piece);
				break;
			case PieceType.Knight:
				res = getThreatenedKnightLocations(piece);
				break;
			case PieceType.Bishop:
				res = getThreatenedBishopLocations(piece);
				break;
			case PieceType.Rook:
				res = getThreatenedRookLocations(piece);
				break;
			case PieceType.Queen:
				res = getThreatenedQueenLocations(piece);
				break;
			default:
				throw new ArgumentException("Piece must be from chess.", nameof(piece));
			}

			return res;
		}

		public bool WouldLeaveInCheck(Move moveOriginal, Side side)
		{
			Board future = Board.DeepCopy();
			//Move moveTested = moveOriginal.DeepCopy(future);
			Move moveTested = new Move(future[moveOriginal.From], moveOriginal.To, future, true);
			future.Moves.Add(moveTested);
			moveTested.DoMove();
		
			return future.IsInCheck(side);
		}

		/// <summary>
		/// Resets the internal cache so that the next time <see cref="GetValidLocations(Piece)"/> is called,
		/// the piece is fully tested. Should only be manually called after a move is made.
		/// </summary>
		public void ResetCache()
		{
			_validCache = null;
			InvalidErrors.Clear();
		}

		private void addError(Tile tile, string error)
		{
			if (!BypassingCheck)
			{
				try
				{
					InvalidErrors.Add(tile, error);
				}
				catch (ArgumentException) // Skip duplicates, that's ok.
				{ }
			}
		}

		private List<Move> getValidKingLocations(Piece piece)
		{
			if (piece.Type != PieceType.King)
			{
				throw new ArgumentException("Piece must be king.", nameof(piece));
			}

			List<Move> res = new List<Move>();

			for (int row = piece.Position.Row - 1; row <= piece.Position.Row + 1; row++)
			{
				for (int col = piece.Position.Column - 1; col <= piece.Position.Column + 1; col++)
				{
					Tile test = new Tile(row, col);

					if (!test.IsValid)
					{
						addError(test, "OFF BOARD");
						continue;
					}

					if (test == piece.Position)
					{
						addError(test, "ORIGINAL POSITION");
						continue;
					}

					Piece targetPiece = Board[test];
					if (targetPiece != null && targetPiece.Side == piece.Side)
					{
						addError(test, "OCCUPIED");
						continue;
					}

					res.Add(new Move(piece, test, Board, true));
				}
			}

			if (!BypassingCheck)
			{
				checkCastling(piece, res);
			}
			
			return res;
		}

		private void checkCastling(Piece piece, List<Move> res)
		{
			Tile qsCastle = piece.Position + new Tile(0, -2);
			Tile ksCastle = piece.Position + new Tile(0, 2);

			if (piece.HasMoved)
			{
				InvalidErrors.Add(qsCastle, "KING MOVED");
				InvalidErrors.Add(ksCastle, "KING MOVED");
				return;
			}

			if (Board.IsThreatened(piece.Position, piece.Side.Opposite()))
			{
				InvalidErrors.Add(qsCastle, "KING IN CHECK");
				InvalidErrors.Add(ksCastle, "KING IN CHECK");
				return;
			}

			if (!qsCastle.IsValid || !ksCastle.IsValid)
			{
				return;
			}

			int row = piece.Position.Row;

			#region QS CASTLE
			Piece qsRook = null;
			for (int i = 0; i < piece.Position.Column - 1; i++)
			{
				Piece p = Board[row, i];
				if (p != null && p.Type == PieceType.Rook)
				{
					qsRook = p;
					break;
				}
			}

			if (qsRook != null)
			{
				if (!qsRook.HasMoved)
				{
					bool allClearPieces = true;
					bool allClearCheck = true;
					for (int i = qsRook.Position.Column + 1; i < piece.Position.Column; i++)
					{
						if (Board[row, i] != null)
						{
							allClearPieces = false;
							break;
						}

						if (i >= qsCastle.Column && Board.IsThreatened(new Tile(row, i), piece.Side.Opposite()))
						{
							allClearCheck = false;
							break;
						}
					}

					if (allClearPieces && allClearCheck)
					{
						res.Add(new MoveCastle(piece, qsRook, qsCastle, Board));
					}
					else if (!allClearCheck)
					{
						InvalidErrors.Add(qsCastle, "THREATENED");
					}
					else
					{
						InvalidErrors.Add(qsCastle, "BLOCKED");
					}
				}
				else
				{
					InvalidErrors.Add(qsCastle, "ROOK MOVED");
				}
			}
			else
			{
				InvalidErrors.Add(qsCastle, "ROOK MISSING");
			}
			#endregion QS CASTLE

			#region KS CASTLE
			Piece ksRook = null;
			for (int i = piece.Position.Column + 1; i < 8; i++)
			{
				Piece p = Board[row, i];
				if (p != null && p.Type == PieceType.Rook)
				{
					ksRook = p;
					break;
				}
			}

			if (ksRook != null)
			{
				if (!ksRook.HasMoved)
				{
					bool allClearPieces = true;
					bool allClearCheck = true;
					for (int i = piece.Position.Column + 1; i < ksRook.Position.Column; i++)
					{
						if (Board[row, i] != null)
						{
							allClearPieces = false;
							break;
						}

						if (i <= ksCastle.Column && Board.IsThreatened(new Tile(row, i), piece.Side.Opposite()))
						{
							allClearCheck = false;
							break;
						}
					}

					if (allClearPieces && allClearCheck)
					{
						res.Add(new MoveCastle(piece, ksRook, ksCastle, Board));
					}
					else if (!allClearCheck)
					{
						InvalidErrors.Add(ksCastle, "THREATENED");
					}
					else
					{
						InvalidErrors.Add(ksCastle, "BLOCKED");
					}
				}
				else
				{
					InvalidErrors.Add(ksCastle, "ROOK MOVED");
				}
			}
			else
			{
				InvalidErrors.Add(ksCastle, "ROOK MISSING");
			}
			#endregion KS CASTLE
		}

		private List<Tile> getThreatenedKingLocations(Piece piece)
		{
			if (piece.Type != PieceType.King)
			{
				throw new ArgumentException("Piece must be king.", nameof(piece));
			}

			List<Tile> res = new List<Tile>();

			for (int row = piece.Position.Row - 1; row <= piece.Position.Row + 1; row++)
			{
				for (int col = piece.Position.Column - 1; col <= piece.Position.Column + 1; col++)
				{
					Tile test = new Tile(row, col);

					if (!test.IsValid)
					{
						continue;
					}

					Piece targetPiece = Board[test];
					if (targetPiece != null && targetPiece.Side == piece.Side)
					{
						continue;
					}

					res.Add(test);
				}
			}
			
			return res;
		}

		private List<Move> getValidQueenLocations(Piece piece)
		{
			if (piece.Type != PieceType.Queen)
			{
				throw new ArgumentException("Piece must be a queen.", nameof(piece));
			}

			addError(piece.Position, "ORIGINAL POSITION");

			List<Move> res = new List<Move>();
			Tile[] expandDirs = new Tile[8] {
				new Tile(-1, -1),
				new Tile(0, -1),
				new Tile(1, -1),
				new Tile(-1, 0),
				new Tile(1, 0),
				new Tile(-1, 1),
				new Tile(0, 1),
				new Tile(1, 1)
			};

			for (int d = 0; d < expandDirs.Length; d++)
			{
				Tile test = piece.Position + expandDirs[d];
				while (test.IsValid)
				{
					Piece targetPiece = Board[test];
					if (targetPiece != null)
					{
						if (targetPiece.Side != piece.Side)
						{
							res.Add(new Move(piece, test, Board, true));
						}
						else
						{
							addError(test, "OCCUPIED");
						}

						break;
					}
					else
					{
						res.Add(new Move(piece, test, Board, true));
					}

					test += expandDirs[d];
				}
			}
			
			return res;
		}

		private List<Tile> getThreatenedQueenLocations(Piece piece)
		{
			if (piece.Type != PieceType.Queen)
			{
				throw new ArgumentException("Piece must be a queen.", nameof(piece));
			}

			List<Tile> res = new List<Tile>();
			Tile[] expandDirs = new Tile[8] {
				new Tile(-1, -1),
				new Tile(0, -1),
				new Tile(1, -1),
				new Tile(-1, 0),
				new Tile(1, 0),
				new Tile(-1, 1),
				new Tile(0, 1),
				new Tile(1, 1)
			};

			for (int d = 0; d < expandDirs.Length; d++)
			{
				Tile test = piece.Position + expandDirs[d];
				while (test.IsValid)
				{
					Piece targetPiece = Board[test];
					if (targetPiece != null)
					{
						if (targetPiece.Side != piece.Side)
						{
							res.Add(test);
						}

						break;
					}
					else
					{
						res.Add(test);
					}

					test += expandDirs[d];
				}
			}
			
			return res;
		}

		private List<Move> getValidRookLocations(Piece piece)
		{
			if (piece.Type != PieceType.Rook)
			{
				throw new ArgumentException("Piece must be a rook.", nameof(piece));
			}

			addError(piece.Position, "ORIGINAL POSITION");

			List<Move> res = new List<Move>();
			Tile[] expandDirs = new Tile[4] {
				new Tile(0, -1),
				new Tile(-1, 0),
				new Tile(1, 0),
				new Tile(0, 1)
			};

			for (int d = 0; d < expandDirs.Length; d++)
			{
				Tile test = piece.Position + expandDirs[d];
				while (test.IsValid)
				{
					Piece targetPiece = Board[test];
					if (targetPiece != null)
					{
						if (targetPiece.Side != piece.Side)
						{
							res.Add(new Move(piece, test, Board, true));
						}
						else
						{
							addError(test, "OCCUPIED");
						}

						break;
					}
					else
					{
						res.Add(new Move(piece, test, Board, true));
					}

					test += expandDirs[d];
				}
			}

			return res;
		}

		private List<Tile> getThreatenedRookLocations(Piece piece)
		{
			if (piece.Type != PieceType.Rook)
			{
				throw new ArgumentException("Piece must be a rook.", nameof(piece));
			}

			List<Tile> res = new List<Tile>();
			Tile[] expandDirs = new Tile[4] {
				new Tile(0, -1),
				new Tile(-1, 0),
				new Tile(1, 0),
				new Tile(0, 1)
			};

			for (int d = 0; d < expandDirs.Length; d++)
			{
				Tile test = piece.Position + expandDirs[d];
				while (test.IsValid)
				{
					Piece targetPiece = Board[test];
					if (targetPiece != null)
					{
						if (targetPiece.Side != piece.Side)
						{
							res.Add(test);
						}

						break;
					}
					else
					{
						res.Add(test);
					}

					test += expandDirs[d];
				}
			}

			return res;
		}

		private List<Move> getValidBishopLocations(Piece piece)
		{
			if (piece.Type != PieceType.Bishop)
			{
				throw new ArgumentException("Piece must be a bishop.", nameof(piece));
			}

			addError(piece.Position, "ORIGINAL POSITION");

			List<Move> res = new List<Move>();
			Tile[] expandDirs = new Tile[4] {
				new Tile(-1, -1),
				new Tile(1, -1),
				new Tile(-1, 1),
				new Tile(1, 1)
			};

			for (int d = 0; d < expandDirs.Length; d++)
			{
				Tile test = piece.Position + expandDirs[d];
				while (test.IsValid)
				{
					Piece targetPiece = Board[test];
					if (targetPiece != null)
					{
						if (targetPiece.Side != piece.Side)
						{
							res.Add(new Move(piece, test, Board, true));
						}
						else
						{
							addError(test, "OCCUPIED");
						}

						break;
					}
					else
					{
						res.Add(new Move(piece, test, Board, true));
					}

					test += expandDirs[d];
				}
			}
			
			return res;
		}

		private List<Tile> getThreatenedBishopLocations(Piece piece)
		{
			if (piece.Type != PieceType.Bishop)
			{
				throw new ArgumentException("Piece must be a bishop.", nameof(piece));
			}

			List<Tile> res = new List<Tile>();
			Tile[] expandDirs = new Tile[4] {
				new Tile(-1, -1),
				new Tile(1, -1),
				new Tile(-1, 1),
				new Tile(1, 1)
			};

			for (int d = 0; d < expandDirs.Length; d++)
			{
				Tile test = piece.Position + expandDirs[d];
				while (test.IsValid)
				{
					Piece targetPiece = Board[test];
					if (targetPiece != null)
					{
						if (targetPiece.Side != piece.Side)
						{
							res.Add(test);
						}

						break;
					}
					else
					{
						res.Add(test);
					}

					test += expandDirs[d];
				}
			}
			
			return res;
		}

		private List<Move> getValidKnightLocations(Piece piece)
		{
			if (piece.Type != PieceType.Knight)

			{
				throw new ArgumentException("Piece must be a knight.", nameof(piece));
			}

			addError(piece.Position, "ORIGINAL POSITION");

			List<Move> res = new List<Move>();

			Tile[] offsets = new Tile[8] {
				new Tile(1, 2),
				new Tile(-1, 2),
				new Tile(1, -2),
				new Tile(-1, -2),
				new Tile(2, 1),
				new Tile(-2, 1),
				new Tile(2, -1),
				new Tile(-2, -1)
			};

			foreach (Tile o in offsets)
			{
				Tile test = piece.Position + o;

				if (!test.IsValid)
				{
					addError(test, "OFF BOARD");
					continue;
				}

				Piece targetPiece = Board[test];
				if (targetPiece != null && targetPiece.Side == piece.Side)
				{
					addError(test, "OCCUPIED");
					continue;
				}

				res.Add(new Move(piece, test, Board, true));
			}
			
			return res;
		}

		private List<Tile> getThreatenedKnightLocations(Piece piece)
		{
			if (piece.Type != PieceType.Knight)

			{
				throw new ArgumentException("Piece must be a knight.", nameof(piece));
			}

			List<Tile> res = new List<Tile>();

			Tile[] offsets = new Tile[8] {
				new Tile(1, 2),
				new Tile(-1, 2),
				new Tile(1, -2),
				new Tile(-1, -2),
				new Tile(2, 1),
				new Tile(-2, 1),
				new Tile(2, -1),
				new Tile(-2, -1)
			};

			foreach (Tile o in offsets)
			{
				Tile test = piece.Position + o;

				if (!test.IsValid)
				{
					continue;
				}

				Piece targetPiece = Board[test];
				if (targetPiece != null && targetPiece.Side == piece.Side)
				{
					continue;
				}

				res.Add(test);
			}
			
			return res;
		}

		private List<Move> getValidPawnLocations(Piece piece)
		{
			if (piece.Type != PieceType.Pawn)
			{
				throw new ArgumentException("Piece must be a pawn.", nameof(piece));
			}

			addError(piece.Position, "ORIGINAL POSITION");

			List<Move> res = new List<Move>();

			int forward = piece.Side == Side.White ? 1 : -1;

			checkPawnForward(piece, res, forward);

			checkPawnDiagonal(piece, res, forward, -1);
			checkPawnDiagonal(piece, res, forward, 1);

			return res;
		}

		private List<Tile> getThreatenedPawnLocations(Piece piece)
		{
			if (piece.Type != PieceType.Pawn)
			{
				throw new ArgumentException("Piece must be a pawn.", nameof(piece));
			}

			List<Tile> res = new List<Tile>();

			int forward = piece.Side == Side.White ? 1 : -1;

			checkPawnDiagonalThreat(piece, res, forward, -1);
			checkPawnDiagonalThreat(piece, res, forward, 1);

			return res;
		}

		private void checkPawnForward(Piece piece, List<Move> res, int forward)
		{
			Tile inFront = piece.Position + new Tile(forward, 0);
			Tile inFrontTwo = piece.Position + new Tile(forward * 2, 0);

			if (!piece.HasMoved && inFrontTwo.IsValid)
			{
				if (Board[inFront] != null)
				{
					addError(inFrontTwo, "BLOCKED");
				}
				else if (Board[inFrontTwo] != null)
				{
					addError(inFrontTwo, "OCCUPIED");
				}
				else
				{
					res.Add(createPawnMove(piece, inFrontTwo));
				}
			}

			if (inFront.IsValid)
			{
				if (Board[inFront] != null)
				{
					addError(inFront, "OCCUPIED");
				}
				else
				{
					res.Add(createPawnMove(piece, inFront));
				}
			}
		}

		private void checkPawnDiagonal(Piece piece, List<Move> res, int forward, int leftRight)
		{
			Tile diagonal = piece.Position + new Tile(forward, leftRight);
			Tile ep = piece.Position + new Tile(0, leftRight);

			if (diagonal.IsValid)
			{
				if (Board[diagonal] == null)
				{
					// en passant check
					if (Board[ep] != null && Board[ep].Type == PieceType.Pawn && Board[ep].PawnJustMovedDouble)
					{
						res.Add(createPawnMove(piece, diagonal));
					}
					else
					{
						addError(diagonal, "EMPTY");
					}
				}
				else if (Board[diagonal].Side == piece.Side)
				{
					addError(diagonal, "OCCUPIED");
				}
				else
				{
					res.Add(createPawnMove(piece, diagonal));
				}
			}
		}

		private void checkPawnDiagonalThreat(Piece piece, List<Tile> res, int forward, int leftRight)
		{
			Tile diagonal = piece.Position + new Tile(forward, leftRight);
			Tile ep = piece.Position + new Tile(0, leftRight);

			if (diagonal.IsValid)
			{
				if (Board[diagonal] == null)
				{
					// en passant check
					if (Board[ep] != null && Board[ep].Type == PieceType.Pawn && Board[ep].PawnJustMovedDouble)
					{
						res.Add(diagonal);
					}
				}
				else if (Board[diagonal].Side != piece.Side)
				{
					res.Add(diagonal);
				}
			}
		}

		private Move createPawnMove(Piece piece, Tile tileTo)
		{
			int promotionRow = piece.Side == Side.White ? 7 : 0;
			if (tileTo.Row == promotionRow)
			{
				return new MovePromotion(piece, tileTo, Board);
			}
			else
			{
				return new Move(piece, tileTo, Board, true);
			}
		}
	}
}
