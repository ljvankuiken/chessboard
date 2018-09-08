using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
	public struct Tile : IEquatable<Tile>
	{
		public int Row
		{ get; private set; }

		public int Column
		{ get; private set; }

		public bool IsValid => Row >= 0 && Row < 8 && Column >= 0 && Column < 8;

		public Tile(int row, int col)
		{
			Row = row;
			Column = col;
		}

		public bool Equals(Tile other)
		{
			return Row == other.Row && Column == other.Column;
		}

		public override bool Equals(object obj)
		{
			if (obj is Tile other)
			{
				return Row == other.Row && Column == other.Column;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return Row ^ Column;

		}

		public override string ToString()
		{
			return Util.GetPosAlgebraic(this);
		}

		public static Tile operator+(Tile a, Tile b)
		{
			return new Tile(a.Row + b.Row, a.Column + b.Column);
		}
		public static Tile operator-(Tile a, Tile b)
		{
			return new Tile(a.Row - b.Row, a.Column - b.Column);
		}

		public static bool operator==(Tile a, Tile b)
		{
			return a.Equals(b);
		}

		public static bool operator!=(Tile a, Tile b)
		{
			return !a.Equals(b);
		}
	}
}
