using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public enum Side
	{
		White,
		Black
	}

    public class Board
    {
		public Piece[,] Layout
		{ get; private set; }
    }
}
