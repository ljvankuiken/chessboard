using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessBoardFormsDisplay
{
	public class InterpolationPictureBox : PictureBox
	{
		public InterpolationMode Interpolation
		{ get; set; }

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.InterpolationMode = Interpolation;
			base.OnPaint(pe);
		}
	}
}
