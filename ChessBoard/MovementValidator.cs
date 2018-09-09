using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public class MovementValidator
	{
		public Board Board
		{ get; }

		public Dictionary<Tile, string> InvalidErrors
		{ get; } = new Dictionary<Tile, string>();

		private List<Move> _validCache;
		private Tile _cachedPosition;
		private PieceType _cachedType;

		public MovementValidator(Board board)
		{
			Board = board;
		}

		public bool IsMovementValid(Piece piece, Tile target)
		{
			List<Move> valid = GetValidLocations(piece);

			return valid == null || valid.Exists(m => m.To == target);
		}

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

		public List<Move> GetValidLocations(Piece piece)
		{
			if (_validCache != null && piece.Type == _cachedType && piece.Position == _cachedPosition)
			{
				return _validCache;
			}
			else
			{
				_validCache = null;
				InvalidErrors.Clear();
			}

			_cachedPosition = piece.Position;
			_cachedType = piece.Type;

			switch (piece.Type)
			{
			case PieceType.King:
				return GetValidKingLocations(piece);
			case PieceType.Pawn:
				return GetValidPawnLocations(piece);
			case PieceType.Knight:
				return GetValidKnightLocations(piece);
			case PieceType.Bishop:
				return GetValidBishopLocations(piece);
			case PieceType.Rook:
				return GetValidRookLocations(piece);
			case PieceType.Queen:
				return GetValidQueenLocations(piece);
			default:
				throw new ArgumentException("Piece must be from chess.", nameof(piece));
			}
		}

		public List<Move> GetValidKingLocations(Piece piece)
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
						InvalidErrors.Add(test, "OFF BOARD");
						continue;
					}

					if (test == piece.Position)
					{
						InvalidErrors.Add(test, "ORIGINAL POSITION");
						continue;
					}

					Piece targetPiece = Board[test];
					if (targetPiece != null && targetPiece.Side == piece.Side)
					{
						InvalidErrors.Add(test, "OCCUPIED");
						continue;
					}

					res.Add(new Move(piece, test, Board));
				}
			}

			checkCastling(piece, res);

			_validCache = res;
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
					bool allClear = true;
					for (int i = qsRook.Position.Column + 1; i < piece.Position.Column; i++)
					{
						if (Board[row, i] != null)
						{
							allClear = false;
							break;
						}
					}

					if (allClear)
					{
						res.Add(new MoveCastle(piece, qsRook, qsCastle, Board));
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
					bool allClear = true;
					for (int i = piece.Position.Column + 1; i < ksRook.Position.Column; i++)
					{
						if (Board[row, i] != null)
						{
							allClear = false;
							break;
						}
					}

					if (allClear)
					{
						res.Add(new MoveCastle(piece, ksRook, ksCastle, Board));
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

		public List<Move> GetValidQueenLocations(Piece piece)
		{
			if (piece.Type != PieceType.Queen)
			{
				throw new ArgumentException("Piece must be a queen.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
							res.Add(new Move(piece, test, Board));
						}
						else
						{
							InvalidErrors.Add(test, "OCCUPIED");
						}

						break;
					}
					else
					{
						res.Add(new Move(piece, test, Board));
					}

					test += expandDirs[d];
				}
			}

			_validCache = res;
			return res;
		}

		public List<Move> GetValidRookLocations(Piece piece)
		{
			if (piece.Type != PieceType.Rook)
			{
				throw new ArgumentException("Piece must be a rook.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
							res.Add(new Move(piece, test, Board));
						}
						else
						{
							InvalidErrors.Add(test, "OCCUPIED");
						}

						break;
					}
					else
					{
						res.Add(new Move(piece, test, Board));
					}

					test += expandDirs[d];
				}
			}

			_validCache = res;
			return res;
		}

		public List<Move> GetValidBishopLocations(Piece piece)
		{
			if (piece.Type != PieceType.Bishop)
			{
				throw new ArgumentException("Piece must be a bishop.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
							res.Add(new Move(piece, test, Board));
						}
						else
						{
							InvalidErrors.Add(test, "OCCUPIED");
						}

						break;
					}
					else
					{
						res.Add(new Move(piece, test, Board));
					}

					test += expandDirs[d];
				}
			}

			_validCache = res;
			return res;
		}

		public List<Move> GetValidKnightLocations(Piece piece)
		{
			if (piece.Type != PieceType.Knight)

			{
				throw new ArgumentException("Piece must be a knight.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
					InvalidErrors.Add(test, "OFF BOARD");
					continue;
				}

				Piece targetPiece = Board[test];
				if (targetPiece != null && targetPiece.Side == piece.Side)
				{
					InvalidErrors.Add(test, "OCCUPIED");
					continue;
				}

				res.Add(new Move(piece, test, Board));
			}

			_validCache = res;
			return res;
		}

		public List<Move> GetValidPawnLocations(Piece piece)
		{
			if (piece.Type != PieceType.Pawn)
			{
				throw new ArgumentException("Piece must be a pawn.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

			List<Move> res = new List<Move>();

			int forward = piece.Side == Side.White ? 1 : -1;

			checkPawnForward(piece, res, forward);

			checkPawnDiagonal(piece, res, forward, -1);
			checkPawnDiagonal(piece, res, forward, 1);

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
					InvalidErrors.Add(inFrontTwo, "BLOCKED");
				}
				else if (Board[inFrontTwo] != null)
				{
					InvalidErrors.Add(inFrontTwo, "OCCUPIED");
				}
				else
				{
					res.Add(new Move(piece, inFrontTwo, Board));
				}
			}

			if (inFront.IsValid)
			{
				if (Board[inFront] != null)
				{
					InvalidErrors.Add(inFront, "OCCUPIED");
				}
				else
				{
					res.Add(new Move(piece, inFront, Board));
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
						res.Add(new Move(piece, diagonal, Board));
					}
					else
					{
						InvalidErrors.Add(diagonal, "EMPTY");
					}
				}
				else if (Board[diagonal].Side == piece.Side)
				{
					InvalidErrors.Add(diagonal, "OCCUPIED");
				}
				else
				{
					res.Add(new Move(piece, diagonal, Board));
				}
			}
		}
	}
}
