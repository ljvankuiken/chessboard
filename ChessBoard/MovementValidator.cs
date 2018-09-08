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
				return null;
			case PieceType.Knight:
				return null;
			case PieceType.Bishop:
				return null;
			case PieceType.Rook:
				return null;
			case PieceType.Queen:
				return GetValidQueenLocations(piece);
			default:
				throw new ArgumentException("Piece must be from chess.", nameof(piece));
			}
		}

		public List<Tile> GetValidKingLocations(Piece piece)
		{
			//InvalidErrors.Clear();

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

			for (int d = 0; d < 8; d++)
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

			return res;
		}
	}
}
