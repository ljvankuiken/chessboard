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
		{ get; }

		public Sprite NotationOverlay
		{ get; }

		public double Scale
		{ get; private set; }

		public List<RenderedPiece> RenderedPieces
		{ get; } = new List<RenderedPiece>();

		public RenderedPiece GrabbedPiece
		{ get; set; }

		public GhostPiece GrabbedGhost
		{ get; set; }

		public Vector2 GrabOffset
		{ get; set; }

		public Board Board
		{ get; }

		public bool EnforceTurns
		{ get; set; }

		public bool ShowNotationOverlay
		{
			get => NotationOverlay.Visible;
			set => NotationOverlay.Visible = value;
		}

		public bool DebugMode
		{
			get => _debugMode;
			set
			{
				foreach (RenderedPiece rp in RenderedPieces)
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

		public List<Rectangle> GrabbedValidLocations
		{ get; } = new List<Rectangle>();

		public ChessBoardGame(Canvas control) : base(control)
		{
			Scale = 0.5;

			BoardSprite = new Sprite(Canvas, "img/qualityboard_1024.png", new Vector2(50, 50), Scale);
			NotationOverlay = new Sprite(Canvas, "img/notationoverlay_1024.png", new Vector2(50, 50), Scale, 0.65);
			Panel.SetZIndex(NotationOverlay.Control, 1100);

			Sprites.Add(BoardSprite);
			Sprites.Add(NotationOverlay);
			
			Board = new Board();
			Board.OnPieceMoved += onPieceMoved;

			SetUpPiecesFromBoard();

			EnforceTurns = false;
			ShowNotationOverlay = true;
		}

		public override void Initialize(RoutedEventArgs e)
		{
			BoardSprite.Initialize();
			NotationOverlay.Initialize();

			foreach (RenderedPiece rp in RenderedPieces)
			{
				rp.Initialize();
			}

			base.Initialize(e);
		}

		public void SetUpPiecesFromBoard()
		{
			RenderedPieces.Clear();

			foreach (Piece p in Board.Pieces)
			{
				if (p != null)
				{
					RenderedPieces.Add(new RenderedPiece(p, BoardSprite, Canvas));
				}
			}
		}

		public void GrabPiece(RenderedPiece piece, MouseButtonEventArgs e)
		{
			if (EnforceTurns && piece.Piece.Side != Board.Turn)
			{
				return;
			}

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

			if (Board.Validator.IsMovementValid(GrabbedPiece.Piece, tile, out Move move))
			{
				Board.Moves.Add(move);
				move.DoMove();

				foreach (RenderedPiece rp in RenderedPieces)
				{
					rp.RenderedPos = getAbsCoords(rp.Piece.Position);
					rp.Refresh();
				}
			}
			else
			{
				GrabbedPiece.RenderedPos = getAbsCoords(GrabbedPiece.Piece.Position);
				GrabbedPiece.Refresh();
			}

			Panel.SetZIndex(GrabbedPiece.Sprite.Control, 10);
			
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

				List<Move> valid = Board.Validator.GetValidLocations(GrabbedPiece.Piece);
				if (valid != null)
				{
					foreach (Move m in valid)
					{
						Rectangle r = new Rectangle() {
							Width = GrabbedPiece.Sprite.ActualWidth,
							Height = GrabbedPiece.Sprite.ActualHeight,
							Fill = new SolidColorBrush(Colors.Blue),
							Opacity = 0.4
						};
						r.SetPos(getAbsCoords(m.To));
						Panel.SetZIndex(r, 1);
						Canvas.Children.Add(r);
						GrabbedValidLocations.Add(r);
					}
				}
			}

			base.Refresh(e);
		}

		private Tile getTile(Vector2 posAbs)
		{
			Vector2 relPos = posAbs - BoardSprite.Position;
			
			int tx = (int)(relPos.X / BoardSprite.ActualWidth * 8.0);
			int ty = 7 - (int)(relPos.Y / BoardSprite.ActualHeight * 8.0);

			return new Tile(ty, tx).ClampValid();
		}

		private Vector2 getAbsCoords(Tile tile)
		{
			return new Vector2(tile.Column / 8.0 * BoardSprite.ActualWidth,
				(7 - tile.Row) / 8.0 * BoardSprite.ActualHeight) + BoardSprite.Position;
		}

		public override string GetDebugText(MouseEventArgs e)
		{
			List<string> lines = new List<string>();

			Vector2 posAbs = e.GetPositionV();
			lines.Add("Abs coords: " + posAbs.ToString());

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

			Piece pointedAtPiece = Board[getTile(posAbs)];

			if (pointedAtPiece != null)
			{
				lines.Add("Piece: " + pointedAtPiece.Side.ToString() + " " + pointedAtPiece.Type.ToString());

				if (!pointedAtPiece.HasMoved)
				{
					lines.Add("Not yet moved");
				}

				if (pointedAtPiece.Type == PieceType.Pawn && pointedAtPiece.PawnJustMovedDouble)
				{
					lines.Add("Open to en passant");
				}

				if (pointedAtPiece.Side != Board.Turn && EnforceTurns)
				{
					lines.Add("Wrong turn");
				}
			}

			if (!EnforceTurns)
			{
				lines.Add("TURNS DISABLED");
			}

			if (Board.IsInCheck(Side.White))
			{
				lines.Add("WHITE IS IN CHECK");
			}
			if (Board.IsInCheck(Side.Black))
			{
				lines.Add("BLACK IS IN CHECK");
			}

			// ---

			string res = "";
			foreach (string l in lines)
			{
				res += l + "\n";
			}

			return res;
		}

		private void onPieceMoved(object sender, PieceMovedEventArgs e)
		{
			// Remove captured pieces
			for (int i = RenderedPieces.Count - 1; i >= 0; i--)
			{
				RenderedPiece rp = RenderedPieces[i];
				if (!Board.Pieces.Contains(rp.Piece))
				{
					rp.Disconnect(Canvas);
					RenderedPieces.RemoveAt(i);
				}
			}

			Board.Validator.ResetCache();
			Board.SwitchTurn();
		}
	}
}
