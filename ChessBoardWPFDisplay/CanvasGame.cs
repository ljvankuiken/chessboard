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
    public class CanvasGame
    {
		public readonly Canvas Canvas;

		public List<Sprite> Sprites = new List<Sprite>();

		public CanvasGame(Canvas canvas)
		{
			Canvas = canvas;
		}

		public virtual void Initialize(RoutedEventArgs e)
		{ }

		public virtual void Refresh(MouseEventArgs e)
		{
			foreach (Sprite sprite in Sprites)
			{
				sprite.Refresh();
			}
		}

		public virtual string GetDebugText(MouseEventArgs e) => "";
    }
}
