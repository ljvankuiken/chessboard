using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoardWPFDisplay
{
	public struct AABB : IEquatable<AABB>
	{
		public double X
		{ get; }

		public double Y
		{ get; }

		public double Width
		{ get; }

		public double Height
		{ get; }

		public Vector2 TopLeft => new Vector2(X, Y);
		public Vector2 TopRight => new Vector2(X + Width, Y);
		public Vector2 BottomLeft => new Vector2(X, Y + Height);
		public Vector2 BottomRight => new Vector2(X + Width, Y + Height);

		public Vector2 Center => new Vector2(X + Width / 2.0, Y + Height / 2.0);

		public AABB(double x, double y, double w, double h)
		{
			X = x;
			Y = y;
			Width = w;
			Height = h;
		}

		public AABB(Vector2 topleft, Vector2 size) : this(topleft.X, topleft.Y, size.X, size.Y)
		{ }

		public bool Equals(AABB other)
		{
			return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
		}

		public override bool Equals(object obj)
		{
			if (obj is AABB other)
			{
				return Equals(other);
			}

			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format($"({X}, {Y}) + ({Width}, {Height})");
		}

		public bool Contains(Vector2 v)
		{
			return X <= v.X && v.X <= X + Width && Y <= v.Y && v.Y <= Y + Height;
		}
	}
}
