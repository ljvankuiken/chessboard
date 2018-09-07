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

		public List<Sprite> Sprites
		{ get; private set; }

		public CanvasGame(Canvas canvas)
		{
			Canvas = canvas;

			Sprites = new List<Sprite>();
		}

		public virtual void Initialize(RoutedEventArgs e)
		{ }

		public void Refresh()
		{
			foreach (Sprite sprite in Sprites)
			{
				sprite.Update();
			}
		}

		public virtual string GetDebugText(MouseEventArgs e) => "";

		public virtual void OnMouseMove(MouseEventArgs e)
		{ }
    }
}
