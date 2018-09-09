using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessBoardWPFDisplay
{
    public static class DisplayUtil
    {
		public static Vector ToVector(this Point point)
		{
			return new Vector(point.X, point.Y);
		}

		public static Point ToPoint(this Vector v)
		{
			return new Point(v.X, v.Y);
		}

		public static Vector2 GetPositionV(this MouseEventArgs e, IInputElement relativeTo = null)
		{
			return e.GetPosition(relativeTo);
		}

		public static void SetPos(this UIElement element, Vector2 pos)
		{
			Canvas.SetLeft(element, pos.X);
			Canvas.SetTop(element, pos.Y);
		}

		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue def = default(TValue))
		{
			try
			{
				return dict[key];
			}
			catch (KeyNotFoundException)
			{
				return def;
			}
		}
    }
}