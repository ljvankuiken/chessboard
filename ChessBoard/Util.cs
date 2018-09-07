using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public static class Util
	{
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
			char colChar = LETTERS[col];

			return colChar.ToString() + (row + 1).ToString();
		}

		public static string GetPosDescriptive(int row, int col, Side side = Side.White)
		{
			string[] COLUMNS = new string[] { "QR", "QN", "QB", "Q", "K", "KB", "KN", "KR" };

			string colStr = COLUMNS[col];

			int relRow = (side == Side.White ? row : 7 - row) + 1;

			return colStr + relRow.ToString();
		}

		public static string GetPosICCF(int row, int col)
		{
			return (col + 1).ToString() + (row + 1).ToString();
		}
	}
}
