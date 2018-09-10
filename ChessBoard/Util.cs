using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public static class Util
	{
		/// <summary>
		/// Returns the opposite of a given <see cref="Side"/>.
		/// </summary>
		/// <param name="side">Side to mirror.</param>
		/// <returns>
		/// <see cref="Side.White"/> if <paramref name="side"/> is <see cref="Side.Black"/>,
		/// <see cref="Side.Black"/> if <paramref name="side"/> is <see cref="Side.White"/>.
		/// </returns>
		public static Side Opposite(this Side side)
		{
			if (side == Side.White)
			{
				return Side.Black;
			}
			else
			{
				return Side.White;
			}
		}

		public static GameStatus GetVictoryStatus(this Side side)
		{
			if (side == Side.White)
			{
				return GameStatus.VictoryWhite;
			}
			else
			{
				return GameStatus.VictoryBlack;
			}
		}

		public static string Abbreviation(this PieceType type, bool english = false)
		{
			switch (type)
			{
			case PieceType.King:
				return "K";
			case PieceType.Queen:
				return "Q";
			case PieceType.Rook:
				return "R";
			case PieceType.Knight:
				return "N";
			case PieceType.Bishop:
				return "B";
			case PieceType.Pawn:
				return english ? "P" : "";
			default:
				return "???";
			}
		}

		public static bool IsVictory(this GameStatus status)
		{
			return status == GameStatus.VictoryWhite || status == GameStatus.VictoryBlack;
		}

		public static bool IsVictoryForSide(this GameStatus status, Side side)
		{
			return status == side.GetVictoryStatus();
		}

		/// <summary>
		/// Clamps an integer between two values.
		/// </summary>
		/// <param name="val">The value to clamp.</param>
		/// <param name="min">The minimum the result can be.</param>
		/// <param name="max">The maximum the result can be.</param>
		/// <returns>
		/// <paramref name="val"/> if it is inside the given range, or 
		/// <paramref name="min"/> or <paramref name="max"/> if not.
		/// </returns>
		public static int Clamp(int val, int min, int max)
		{
			if (val < min)
			{
				return min;
			}
			else if (val > max)
			{
				return max;
			}
			else
			{
				return val;
			}
		}

		public static bool UniqueAmongAll<TElement, TEval>(this IEnumerable<TElement> enumerable, Func<TElement, TEval> getter)
		{
			List<TEval> results = new List<TEval>();

			foreach (TElement t in enumerable)
			{
				TEval eval = getter(t);
				if (results.Contains(eval))
				{
					return false;
				}
				else
				{
					results.Add(eval);
				}
			}

			return true;
		}
	}
}
