using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessBoard;

namespace ChessBoardWPFDisplay
{
    public class ChessBoardGame : CanvasGame
    {
		public Sprite BoardSprite
		{ get; private set; }

		public double Scale
		{ get; private set; }

		public readonly List<RenderedPiece> Pieces = new List<RenderedPiece>();

		public RenderedPiece GrabbedPiece
		{ get; set; }

		public GhostPiece GrabbedGhost
		{ get; set; }

		public Vector2 GrabOffset
		{ get; set; }

		public Board Board
		{ get; private set; }

		public bool DebugMode
		{
			get => _debugMode;
			set
			{
				foreach (RenderedPiece rp in Pieces)
				{
					rp.ShowDebug = value;
					rp.Refresh();
				}

				if (GrabbedGhost != null)
				{
					GrabbedGhost.ShowDebug = value;
					GrabbedGhost.Refresh();
				}

				_debugMode = value;
			}
		}
		private bool _debugMode;

		public readonly List<Rectangle> GrabbedValidLocations = new List<Rectangle>();

		public ChessBoardGame(Canvas control) : base(control)
		{
			Scale = 0.5;

			BoardSprite = new Sprite(Canvas, "img/qualityboard_1024.png", new Point(50, 50), Scale);
			BoardSprite.Control.Tag = "\"QUALITY\" CHESS BOARD";

			Sprites.Add(BoardSprite);
			
			Board = new Board();
			SetUpPiecesFromBoard();
		}

		public override void Initialize(RoutedEventArgs e)
		{
			BoardSprite.Initialize();

			foreach (RenderedPiece rp in Pieces)
			{
				rp.Initialize();
			}

			base.Initialize(e);
		}

		public void SetUpPiecesFromBoard()
		{
			Pieces.Clear();

			foreach (Piece p in Board.Layout)
			{
				if (p != null)
				{
					Pieces.Add(new RenderedPiece(p.Type, p.Side, p.Position, BoardSprite, Canvas));
				}
			}
		}

		public void GrabPiece(RenderedPiece piece, MouseButtonEventArgs e)
		{
			GrabbedPiece = piece;
			GrabOffset = e.GetPosition(piece.Sprite.Control);
			Panel.SetZIndex(GrabbedPiece.Sprite.Control, 1000);

			foreach (Rectangle r in GrabbedValidLocations)
			{
				Canvas.Children.Remove(r);
			}
			GrabbedValidLocations.Clear();

			GrabbedGhost = new GhostPiece(GrabbedPiece, Canvas);
			GrabbedGhost.Initialize();
		}

		public void DropPiece(Point boardPos)
		{
			if (GrabbedPiece == null)
			{
				return;
			}

			Vector2 posCenter = GrabbedPiece.RenderedPos + GrabbedPiece.Sprite.ActualSize / 2.0;
			Tile tile = getTile(posCenter);

			List<Tile> valid = Board.Validator.GetValidLocations(GrabbedPiece.Piece);

			if (valid == null || valid.Contains(tile))
			{
				foreach (RenderedPiece rp in Pieces)
				{
					if (rp.Piece.Position == tile)
					{
						rp.Disconnect(Canvas);
					}
				}

				Board.MovePiece(GrabbedPiece.Piece, tile);
				GrabbedPiece.RenderedPos = getAbsCoords(tile);
			}
			else
			{
				GrabbedPiece.RenderedPos = getAbsCoords(GrabbedPiece.Piece.Position);
			}

			Panel.SetZIndex(GrabbedPiece.Sprite.Control, 10);

			GrabbedPiece.Refresh();

			GrabbedPiece = null;

			GrabbedGhost.Disconnect(Canvas);
			GrabbedGhost = null;

			foreach (Rectangle r in GrabbedValidLocations)
			{
				Canvas.Children.Remove(r);
			}
			GrabbedValidLocations.Clear();
		}

		public override void Refresh(MouseEventArgs e)
		{
			if (GrabbedPiece != null)
			{
				GrabbedPiece.RenderedPos = e.GetPositionV() - GrabOffset;
				GrabbedPiece.Refresh();

				GrabbedGhost.Refresh();

				foreach (Rectangle r in GrabbedValidLocations)
				{
					Canvas.Children.Remove(r);
				}
				GrabbedValidLocations.Clear();

				List<Tile> valid = Board.Validator.GetValidLocations(GrabbedPiece.Piece);
				if (valid != null)
				{
					foreach (Tile t in valid)
					{
						Rectangle r = new Rectangle() {
							Width = GrabbedPiece.Sprite.ActualWidth,
							Height = GrabbedPiece.Sprite.ActualHeight,
							Fill = new SolidColorBrush(Colors.Blue),
							Opacity = 0.4
						};
						r.SetPos(getAbsCoords(t));
						Panel.SetZIndex(r, 1);
						Canvas.Children.Add(r);
						GrabbedValidLocations.Add(r);
					}
				}
			}

			base.Refresh(e);
		}

		private Tile getTile(Vector2 pos)
		{
			Vector2 relPos = pos - BoardSprite.Position;
			
			int tx = (int)(relPos.X / BoardSprite.ActualWidth * 8.0);
			int ty = 7 - (int)(relPos.Y / BoardSprite.ActualHeight * 8.0);

			return new Tile(ty, tx);
		}

		private Vector2 getAbsCoords(Tile tile)
		{
			return new Vector2(tile.Column / 8.0 * BoardSprite.ActualWidth,
				(7 - tile.Row) / 8.0 * BoardSprite.ActualHeight) + BoardSprite.Position;
		}

		public override string GetDebugText(MouseEventArgs e)
		{
			List<string> lines = new List<string>();

			Vector2 posRel = e.GetPosition(BoardSprite.Control);
			lines.Add("Board coords: " + posRel.ToString());

			int tileX = Math.Min((int)(posRel.X / BoardSprite.ActualWidth * 8), 7);
			int tileY = 7 - Math.Min((int)(posRel.Y / BoardSprite.ActualHeight * 8), 7);

			lines.Add("Tile: " + Util.GetPosAlgebraic(tileY, tileX));

			if (GrabbedPiece != null)
			{
				lines.Add("Grabbed Piece: " + GrabbedPiece.Piece.ToString());

				Vector2 posCenter = GrabbedPiece.RenderedPos + GrabbedPiece.Sprite.ActualSize / 2.0;
				Tile tile = getTile(posCenter);

				lines.Add("(would move to " + tile.GetPosAlgebraic() + ")");

				if (!Board.Validator.IsMovementValid(GrabbedPiece.Piece, tile))
				{
					lines.Add("(" + Board.Validator.InvalidErrors.GetOrDefault(tile, "INVALID") + ")");
				}
			}

			string res = "";
			foreach (string l in lines)
			{
				res += l + "\n";
			}

			return res;
		}
	}
}
