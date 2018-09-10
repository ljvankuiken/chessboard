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
		{ get; }

		public int Column
		{ get; }

		public static Tile UnitR => new Tile(1, 0);
		public static Tile UnitC => new Tile(0, 1);
		public static Tile UnitRC => new Tile(1, 1);

		public Tile RowOnly => new Tile(Row, 0);
		public Tile ColumnOnly => new Tile(0, Column);

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

		public Tile Abs()
		{
			return new Tile(Math.Abs(Row), Math.Abs(Column));
		}

		public Tile ClampValid()
		{
			return new Tile(Util.Clamp(Row, 0, 7), Util.Clamp(Column, 0, 7));
		}

		public char FileLetter()
		{
			const string LETTERS = "abcdefgh";
			return LETTERS[Column];
		}

		public string FileEnglishAbbrev()
		{
			string[] COLUMNS = new string[] { "QR", "QN", "QB", "Q", "K", "KB", "KN", "KR" };

			return COLUMNS[Column];
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
			try
			{
				return ToStringAlgebraic();
			}
			catch (IndexOutOfRangeException)
			{
				return $"[OFF-BOARD: COL {Column} ROW {Row}]";
			}
		}

		public string ToStringAlgebraic()
		{
			return ToStringCoordinate().ToLower();
		}

		public string ToStringCoordinate()
		{
			const string LETTERS = "ABCDEFGH";
			char colChar = LETTERS[Util.Clamp(Column, 0, 7)];

			return colChar.ToString() + Util.Clamp(Row + 1, 1, 8).ToString();
		}

		public string ToStringEnglish(Side side = Side.White)
		{
			int relRow = Util.Clamp(side == Side.White ? Row : 7 - Row, 0, 7) + 1;

			return FileEnglishAbbrev() + relRow.ToString();
		}

		public string ToStringICCF()
		{
			return (Column + 1).ToString() + (Row + 1).ToString();
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
