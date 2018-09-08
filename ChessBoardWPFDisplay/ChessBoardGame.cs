using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
				_debugMode = value;
			}
		}
		private bool _debugMode;

		public ChessBoardGame(Canvas control) : base(control)
		{
			Scale = 0.5;

			BoardSprite = new Sprite(Canvas, "img/qualityboard_1024.png", new Point(50, 50), Scale);
			BoardSprite.Control.Tag = "\"QUALITY\" CHESS BOARD";

			Sprites.Add(BoardSprite);

			//SetUpPieces();
			Board = new Board();
			Board.Reset();
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
		}

		public void DropPiece(Point boardPos)
		{
			if (GrabbedPiece == null)
			{
				return;
			}

			Vector2 posCenter = GrabbedPiece.RenderedPos + GrabbedPiece.Sprite.ActualSize / 2.0;
			Tile tile = getCoords(posCenter);
			GrabbedPiece.Piece.Position = tile;
			GrabbedPiece.RenderedPos = new Vector2(tile.Column / 8.0 * BoardSprite.ActualWidth, (7 - tile.Row) / 8.0 * BoardSprite.ActualHeight) + BoardSprite.Position;

			Panel.SetZIndex(GrabbedPiece.Sprite.Control, 10);

			GrabbedPiece.Refresh();

			GrabbedPiece = null;
		}

		public override void Refresh(MouseEventArgs e)
		{
			if (GrabbedPiece != null)
			{
				GrabbedPiece.RenderedPos = e.GetPositionV() - GrabOffset;
				GrabbedPiece.Refresh();
			}

			base.Refresh(e);
		}

		private Tile getCoords(Vector2 pos)
		{
			Vector2 relPos = pos - BoardSprite.Position;
			
			int tx = (int)(relPos.X / BoardSprite.ActualWidth * 8.0);
			int ty = 7 - (int)(relPos.Y / BoardSprite.ActualHeight * 8.0);

			return new Tile(ty, tx);
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
				Tile tile = getCoords(posCenter);

				lines.Add("(at " + tile.GetPosAlgebraic() + ")");
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
