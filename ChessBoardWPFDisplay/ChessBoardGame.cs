using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChessBoardWPFDisplay
{
    public class ChessBoardGame : CanvasGame
    {
		public Sprite Board
		{ get; private set; }

		public ChessBoardGame(Canvas control) : base(control)
		{

		}

		public override void Initialize(RoutedEventArgs e)
		{
			Board = new Sprite(Canvas, "img/shittyboard_1024.png", new Vector2(50), 0.5);
			Board.Control.Tag = "SHITTY CHESS BOARD";
			Board.Initialize();

			Sprites.Add(Board);

			base.Initialize(e);
		}
    }
}
