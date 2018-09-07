using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

		public double Width => Size.X;
		public double Height => Size.Y;

		public double ActualWidth => Size.X * Scale;
		public double ActualHeight => Size.Y * Scale;

		public double Scale
		{ get; set; }

		public Sprite(Canvas canvas, string path, Vector2 pos, double scale = 1.0, BitmapScalingMode scaling = BitmapScalingMode.NearestNeighbor)
		{
			Uri uri = new Uri("pack://application:,,,/" + path);
			BitmapImage bmp = new BitmapImage(uri);
			Control = new Image()
			{
				Source = bmp,
				Stretch = Stretch.Uniform,
			};
			canvas.Children.Add(Control);

			RenderOptions.SetBitmapScalingMode(Control, scaling);

			Position = pos;
			Scale = scale;
			Size = new Vector2(bmp.PixelWidth, bmp.PixelHeight);
		}

		public virtual void Initialize()
		{
			Refresh();
		}

		public virtual void Refresh()
		{
			Canvas.SetLeft(Control, Position.X);
			Canvas.SetTop(Control, Position.Y);

			Control.Width = Size.X * Scale;
			Control.Height = Size.Y * Scale;
		}
    }
}
