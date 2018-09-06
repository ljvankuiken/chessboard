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

		public static string GetChessPos(int row, int col)
		{
			const string LETTERS = "abcdefgh";
			char colChar = LETTERS[col];

			return colChar.ToString() + (row + 1).ToString();
		}
	}
}
