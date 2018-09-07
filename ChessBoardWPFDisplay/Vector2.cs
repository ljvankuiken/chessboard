using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoardWPFDisplay
{
    public struct Vector2 : IEquatable<Vector2>
    {
		public double X
		{ get; set; }

		public double Y
		{ get; set; }

		public static Vector2 Zero => new Vector2(0, 0);

		public static Vector2 UnitX => new Vector2(1, 0);
		public static Vector2 UnitY => new Vector2(0, 1);
		public static Vector2 UnitXY => new Vector2(1, 1);

		public Vector2 OnlyX => new Vector2(X, 0);
		public Vector2 OnlyY => new Vector2(0, Y);

		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector2(double xy)
		{
			X = xy;
			Y = xy;
		}

		public double DistanceTo(Vector2 other)
		{
			double dx = X - other.X;
			double dy = Y - other.Y;

			return Math.Sqrt(dx * dx + dy * dy);
		}

		// IEquatable<Vector2>
		public bool Equals(Vector2 other) => X == other.X && Y == other.Y;

		public override bool Equals(object obj)
		{
			if (obj is Vector2)
			{
				Vector2 other = (Vector2)obj;
				return X == other.X && Y == other.Y;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override string ToString()
		{
			return "(" + X.ToString() + ", " + Y.ToString() + ")";
		}

		public static Vector2 operator+(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator-(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
		}

		public static Vector2 operator*(Vector2 a, double k)
		{
			return new Vector2(a.X * k, a.Y * k);
		}

		public static Vector2 operator/(Vector2 a, double k)
		{
			return new Vector2(a.X / k, a.Y / k);
		}

		public static bool operator==(Vector2 a, Vector2 b)
		{
			return a.Equals(b);
		}

		public static bool operator!=(Vector2 a, Vector2 b)
		{
			return !a.Equals(b);
		}
	}
}
