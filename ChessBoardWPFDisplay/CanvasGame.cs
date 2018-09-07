using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

		public void Refresh()
		{
			foreach (Sprite sprite in Sprites)
			{

			}
		}
    }
}
