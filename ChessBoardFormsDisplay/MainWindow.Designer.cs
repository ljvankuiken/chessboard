namespace ChessBoardFormsDisplay
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.interpolationPictureBox1 = new ChessBoardFormsDisplay.InterpolationPictureBox();
			((System.ComponentModel.ISupportInitialize)(this.interpolationPictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// interpolationPictureBox1
			// 
			this.interpolationPictureBox1.Image = global::ChessBoardFormsDisplay.Properties.Resources.simpleboard_1024;
			this.interpolationPictureBox1.Interpolation = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			this.interpolationPictureBox1.Location = new System.Drawing.Point(12, 12);
			this.interpolationPictureBox1.Name = "interpolationPictureBox1";
			this.interpolationPictureBox1.Size = new System.Drawing.Size(572, 537);
			this.interpolationPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.interpolationPictureBox1.TabIndex = 0;
			this.interpolationPictureBox1.TabStop = false;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.interpolationPictureBox1);
			this.Name = "MainWindow";
			this.Text = "Chess Board";
			((System.ComponentModel.ISupportInitialize)(this.interpolationPictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private InterpolationPictureBox interpolationPictureBox1;
	}
}

