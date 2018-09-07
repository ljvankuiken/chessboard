using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessBoardWPFDisplay
{
    public class Sprite
    {
		public Image Control
		{ get; set; }

		public Vector2 Position
		{ get; set; }

		public Vector2 Size
		{ get; private set; }

		public double Scale
		{ get; set; }

		static Sprite()
		{
		}

		public Sprite(string path, Vector2 pos, double scale = 1.0)
		{
			Uri uri = new Uri("pack://application:,,,/" + path);
			BitmapImage bmp = new BitmapImage(uri);
			Control = new Image()
			{
				Source = bmp,
				Stretch = Stretch.Uniform,
			};

			RenderOptions.SetBitmapScalingMode(Control, BitmapScalingMode.NearestNeighbor);

			Position = pos;
			Scale = scale;
			Size = new Vector2(bmp.PixelWidth, bmp.PixelHeight);
		}

		public void Update(Canvas canvas)
		{
			Canvas.SetLeft(Control, Position.X);
			Canvas.SetTop(Control, Position.Y);

			Control.Width = Size.X * Scale;
			Control.Height = Size.Y * Scale;
		}
    }
}
