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

		public static string GetPosAlgebraic(int row, int col)
		{
			const string LETTERS = "abcdefgh";
			char colChar = LETTERS[Clamp(col, 0, 7)];

			return colChar.ToString() + Clamp(row + 1, 1, 8).ToString();
		}
		public static string GetPosAlgebraic(this Tile tile)
		{
			return GetPosAlgebraic(tile.Row, tile.Column);
		}

		public static string GetPosDescriptive(int row, int col, Side side = Side.White)
		{
			string[] COLUMNS = new string[] { "QR", "QN", "QB", "Q", "K", "KB", "KN", "KR" };

			string colStr = COLUMNS[Clamp(col, 0, 7)];

			int relRow = Clamp(side == Side.White ? row : 7 - row, 0, 7) + 1;

			return colStr + relRow.ToString();
		}
		public static string GetPosDescriptive(this Tile tile, Side side = Side.White)
		{
			return GetPosDescriptive(tile.Row, tile.Column, side);
		}

		public static string GetPosICCF(int row, int col)
		{
			return (col + 1).ToString() + (row + 1).ToString();
		}
		public static string GetPosICCF(this Tile tile)
		{
			return GetPosICCF(tile.Row, tile.Column);
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
	}
}
