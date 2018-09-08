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
		{ get; private set; }

		public readonly Dictionary<Tile, string> InvalidErrors = new Dictionary<Tile, string>();

		private List<Tile> _validCache;
		private Tile _cachedPosition;
		private PieceType _cachedType;

		public MovementValidator(Board board)
		{
			Board = board;
		}

		public bool IsMovementValid(Piece piece, Tile target)
		{
			List<Tile> valid = GetValidLocations(piece);

			return valid == null || valid.Contains(target);
		}

		public List<Tile> GetValidLocations(Piece piece)
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

		public List<Tile> GetValidKingLocations(Piece piece)
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
					Tile t = new Tile(row, col);

					if (!t.IsValid)
					{
						InvalidErrors.Add(t, "OFF BOARD");
						continue;
					}

					if (t == piece.Position)
					{
						InvalidErrors.Add(t, "ORIGINAL POSITION");
						continue;
					}

					Piece targetPiece = Board[t];
					if (targetPiece != null && targetPiece.Side == piece.Side)
					{
						InvalidErrors.Add(t, "OCCUPIED");
						continue;
					}

					res.Add(t);
				}
			}

			_validCache = res;
			return res;
		}

		public List<Tile> GetValidQueenLocations(Piece piece)
		{
			if (piece.Type != PieceType.Queen)
			{
				throw new ArgumentException("Piece must be a queen.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
						else
						{
							InvalidErrors.Add(test, "OCCUPIED");
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

			_validCache = res;
			return res;
		}

		public List<Tile> GetValidRookLocations(Piece piece)
		{
			if (piece.Type != PieceType.Rook)
			{
				throw new ArgumentException("Piece must be a rook.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
						else
						{
							InvalidErrors.Add(test, "OCCUPIED");
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

			_validCache = res;
			return res;
		}

		public List<Tile> GetValidBishopLocations(Piece piece)
		{
			if (piece.Type != PieceType.Bishop)
			{
				throw new ArgumentException("Piece must be a bishop.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
						else
						{
							InvalidErrors.Add(test, "OCCUPIED");
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

			_validCache = res;
			return res;
		}

		public List<Tile> GetValidKnightLocations(Piece piece)
		{
			if (piece.Type != PieceType.Knight)

			{
				throw new ArgumentException("Piece must be a knight.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

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
					InvalidErrors.Add(test, "OFF BOARD");
					continue;
				}

				Piece targetPiece = Board[test];
				if (targetPiece != null && targetPiece.Side == piece.Side)
				{
					InvalidErrors.Add(test, "OCCUPIED");
					continue;
				}

				res.Add(test);
			}

			_validCache = res;
			return res;
		}

		public List<Tile> GetValidPawnLocations(Piece piece)
		{
			if (piece.Type != PieceType.Pawn)
			{
				throw new ArgumentException("Piece must be a pawn.", nameof(piece));
			}

			InvalidErrors.Add(piece.Position, "ORIGINAL POSITION");

			List<Tile> res = new List<Tile>();

			int forward = piece.Side == Side.White ? 1 : -1;

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
					res.Add(inFrontTwo);
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
					res.Add(inFront);
				}
			}

			Tile diagonalMinus = piece.Position + new Tile(forward, -1);
			Tile diagonalPlus = piece.Position + new Tile(forward, 1);

			Tile epMinus = piece.Position + new Tile(0, -1);
			Tile epPlus = piece.Position + new Tile(0, 1);

			if (diagonalMinus.IsValid)
			{
				if (Board[diagonalMinus] == null)
				{
					if (Board[epMinus] != null && Board[epMinus].Type == PieceType.Pawn && Board[epMinus].PawnJustMovedDouble)
					{
						res.Add(diagonalMinus);
					}
					else
					{
						InvalidErrors.Add(diagonalMinus, "EMPTY");
					}
				}
				else if (Board[diagonalMinus].Side == piece.Side)
				{
					InvalidErrors.Add(diagonalMinus, "OCCUPIED");
				}
				else
				{
					res.Add(diagonalMinus);
				}
			}

			if (diagonalPlus.IsValid)
			{
				if (Board[diagonalPlus] == null)
				{
					if (Board[epPlus] != null && Board[epPlus].Type == PieceType.Pawn && Board[epPlus].PawnJustMovedDouble)
					{
						res.Add(diagonalPlus);
					}
					else
					{
						InvalidErrors.Add(diagonalPlus, "EMPTY");
					}
				}
				else if (Board[diagonalPlus].Side == piece.Side)
				{
					InvalidErrors.Add(diagonalPlus, "OCCUPIED");
				}
				else
				{
					res.Add(diagonalPlus);
				}
			}

			return res;
		}
	}
}
