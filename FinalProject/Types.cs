using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinalProject {
  public enum Player {
    Player1,
    Player2
  }

  public enum PieceType {
    Man,
    King
  }

  public class ForcedMove {
    public Point MoveLocation;
    public Piece MovePiece;
    public List<Piece> RemovePieces = new List<Piece>();

    public ForcedMove(Point location, Piece piece, Piece remove) {
      MoveLocation = location;
      MovePiece = piece;
      RemovePieces.Add(remove);
    }
  }
}
