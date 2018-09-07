using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessBoardWPFDisplay
{
    public static class DisplayUtil
    {
		public static Vector2 ToVector2(this Point point)
		{
			return new Vector2(point.X, point.Y);
		}

		public static Point ToPoint(this Vector2 v)
		{
			return new Point(v.X, v.Y);
		}
    }
}
