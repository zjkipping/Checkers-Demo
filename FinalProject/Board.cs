using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinalProject {
  class Board {
    public List<Tile> Tiles { get => tiles; set => tiles = value; }
    public List<Piece> Pieces { get => pieces; set => pieces = value; }
    public Player player = Player.Player2;

    private List<Tile> tiles = new List<Tile>();
    private List<Piece> pieces = new List<Piece>();
    private Canvas canvas;
    private Piece dragging;
    private Tile over_tile;
    private List<ForcedMove> forcedMoves = new List<ForcedMove>();

    public Board(Canvas canvas) {
      this.canvas = canvas;

      CreateTiles();
      tiles.ForEach(delegate (Tile tile) {
        Canvas.SetTop(tile.Shape, tile.Position.Y * 100);
        Canvas.SetLeft(tile.Shape, tile.Position.X * 100);
        Canvas.SetZIndex(tile.Shape, 0);
        canvas.Children.Add(tile.Shape);
      });

      CreatePieces();
      pieces.ForEach(delegate (Piece piece) {
        Canvas.SetTop(piece.Shape, (piece.Position.Y * 100) + piece.Shape.Height / 3 / 2);
        Canvas.SetLeft(piece.Shape, (piece.Position.X * 100) + piece.Shape.Width / 3 / 2);
        Canvas.SetZIndex(piece.Shape, 1);
        canvas.Children.Add(piece.Shape);
      });

      System.Windows.Application.Current.MainWindow.MouseUp += MainWindow_MouseUp;
      System.Windows.Application.Current.MainWindow.MouseMove += MainWindow_MouseMove;
    }

    public void NextTurn() {
      player = player == Player.Player1 ? Player.Player2 : Player.Player1;
      CheckForced();
      tiles.ForEach(tile => tile.Shape.Stroke = null);
      forcedMoves.ForEach(f => {
        Console.WriteLine("A Force");
        Tile tile = tiles.Find(t => t.Position.Equals(f.MoveLocation));
        if (tile != null) {
          tile.Shape.Stroke = new SolidColorBrush {
            Color = Color.FromRgb(0, 255, 0)
          };
        }
      });
    }

    public bool CheckValidMove(Point new_position, Point old_position) {
      Point difference = new Point(new_position.X - old_position.X, new_position.Y - old_position.Y);

      // checking for position to be a real move space
      if ((new_position.X + new_position.Y) % 2 == 0) {
        return false;
      }

      //checking for new position to be backwards
      if ((difference.Y < 0 && player == Player.Player1) || (difference.Y > 0 && player == Player.Player2)) {
        return false;
      }

      // checking for other pieces here
      if (pieces.Find(piece => piece.Position.Equals(new_position)) != null) {
        return false;
      }

      // checking for new position to be in a 1 move space around the old position
      if (difference.X > 1 || difference.X < -1 || difference.Y > 1 || difference.Y < -1) {
        return false;
      }

      return true;
    }

    public void CheckForced() {
      forcedMoves.Clear();
      pieces.ForEach(piece => {
        if (piece.Owner == player) {
          Constants.CARDINALS.ForEach(delegate (Point direction) {
            Point directed_point = new Point(piece.Position.X + direction.X, piece.Position.Y + direction.Y);
            Piece jump_piece = pieces.Find(other => other.Owner != player && other.Position.Equals(directed_point));
            if (jump_piece != null) {
              CheckDirectedJump(piece, direction, jump_piece);
            }
          });
        }
      });
    }

    public void CheckDirectedJump(Piece piece, Point direction, Piece jump_piece, ForcedMove forcedMove = null) {
      Point directed_jump_point = new Point(jump_piece.Position.X + direction.X, jump_piece.Position.Y + direction.Y);
      if (pieces.Find(other => other.Position.Equals(directed_jump_point)) == null && directed_jump_point.X >= 0 && directed_jump_point.X < 8 && directed_jump_point.Y >= 0 && directed_jump_point.Y < 8) {
        Point difference = new Point(directed_jump_point.X - piece.Position.X, directed_jump_point.Y - piece.Position.Y);
        if ((player == Player.Player2 && difference.Y < 0) || (player == Player.Player1 && difference.Y > 0)) {
          if (forcedMove == null) {
            forcedMove = new ForcedMove(directed_jump_point, piece, jump_piece);
          } else {
            forcedMove.MoveLocation = directed_jump_point;
            forcedMove.RemovePieces.Add(jump_piece);
          }
          Point directed_point = new Point(directed_jump_point.X + direction.X, directed_jump_point.Y + direction.Y);
          Piece next_jump_piece = pieces.Find(other => other.Owner != player && other.Position.Equals(directed_point));
          if (next_jump_piece != null) {
            CheckDirectedJump(piece, direction, next_jump_piece, forcedMove);
          } else {
            forcedMoves.Add(forcedMove);
          }
        }
      } else {
        if (forcedMove != null) {
          forcedMoves.Add(forcedMove);
        }
      }
    }

    private void MainWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
      if (dragging != null) {
        Point mousePoint = Mouse.GetPosition(canvas);
        Point mousePosition = new Point((mousePoint.X - mousePoint.X % 100) / 100, (mousePoint.Y - mousePoint.Y % 100) / 100);

        if (over_tile != null) {
          over_tile.Shape.Stroke = null;
        }
        over_tile = tiles.Find(tile => tile.Position.Equals(mousePosition));
        if (over_tile != null) {
          over_tile.Shape.Stroke = new SolidColorBrush {
            Color = Color.FromRgb(255, 255, 0)
          };
        }

        Canvas.SetTop(dragging.Shape, mousePoint.Y - dragging.Shape.ActualHeight / 2);
        Canvas.SetLeft(dragging.Shape, mousePoint.X - dragging.Shape.ActualWidth / 2);
      }
    }

    private void MainWindow_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      if (dragging != null) {
        bool nextTurn = false;
        Point mousePoint = Mouse.GetPosition(canvas);
        Point new_location = new Point((mousePoint.X - mousePoint.X % 100) / 100, (mousePoint.Y - mousePoint.Y % 100) / 100);
        if (new_location.X >= 0 && new_location.X < 8 && new_location.Y >= 0 && new_location.Y < 8) {
          if (forcedMoves.Count > 0) {
            forcedMoves.ForEach(fm => {
              if (new_location.Equals(fm.MoveLocation) && dragging.Equals(fm.MovePiece)) {
                dragging.Position = new_location;
                nextTurn = true;
                fm.RemovePieces.ForEach(piece => {
                  canvas.Children.Remove(piece.Shape);
                  pieces.Remove(piece);
                });
              }
            });
          } else if (CheckValidMove(new_location, dragging.Position)) {
            dragging.Position = new_location;
            nextTurn = true;
          }
        }
        Canvas.SetTop(dragging.Shape, (dragging.Position.Y * 100) + dragging.Shape.Height / 3 / 2);
        Canvas.SetLeft(dragging.Shape, (dragging.Position.X * 100) + dragging.Shape.Width / 3 / 2);
        Canvas.SetZIndex(dragging.Shape, 1);
        dragging.Shape.Stroke = new SolidColorBrush {
          Color = Color.FromRgb(255, 255, 255)
        };
        dragging = null;
        if (over_tile != null) {
          over_tile.Shape.Stroke = null;
          over_tile = null;
        }
        if (nextTurn) {
          NextTurn();
        }
      }
    }

    private void CreateTiles() {
      for (int x = 0; x < 8; x++) {
        for (int y = 0; y < 8; y++) {
          tiles.Add(new Tile(new Point(x, y)));
        }
      }
    }

    private void CreatePieces() {
      for (int y = 0; y < 3; y++) {
        for (int x = 0; x < 8; x++) {
          if (((x + 1) + y) % 2 == 0) {
            Piece new_piece = new Piece(Player.Player1, new Point(x, y));
            new_piece.Shape.IsMouseDirectlyOverChanged += Shape_IsMouseDirectlyOverChanged;
            new_piece.Shape.MouseDown += Shape_MouseDown;
            pieces.Add(new_piece);
          }
        }
      }

      for (int y = 7; y > 4; y--) {
        for (int x = 7; x >= 0; x--) {
          if (((x + 1) + y) % 2 == 0) {
            Piece new_piece = new Piece(Player.Player2, new Point(x, y));
            new_piece.Shape.IsMouseDirectlyOverChanged += Shape_IsMouseDirectlyOverChanged;
            new_piece.Shape.MouseDown += Shape_MouseDown;
            pieces.Add(new_piece);
          }
        }
      }
    }

    private void Shape_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      Piece piece = pieces.Find(p => p.Shape.Equals((Shape)sender));
      if (piece.Owner == player) {
        dragging = piece;
        Canvas.SetZIndex(dragging.Shape, 2);
      }
    }

    private void Shape_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e) {
      Piece piece = pieces.Find(p => p.Shape.Equals((Shape)sender));
      if (piece.Owner == player) {
        if (piece.Shape.IsMouseOver) {
          piece.Shape.Stroke = new SolidColorBrush {
            Color = Color.FromRgb(255, 255, 0)
          };
        } else {
          piece.Shape.Stroke = new SolidColorBrush {
            Color = Color.FromRgb(255, 255, 255)
          };
        }
      }
    }
  }
}
