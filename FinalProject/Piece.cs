using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FinalProject {
  public class Piece: BoardObject {
    public Player Owner { get => owner; set => owner = value; }
    public PieceType Type { get => type; set => type = value; }

    private Player owner;
    private PieceType type;

    public Piece(Player owner, Point position) {
      this.owner = owner;
      Position = position;
      type = PieceType.Man;
      Shape = new Ellipse {
        Width = 75,
        Height = 75,
        Fill = new SolidColorBrush {
          Color = owner == Player.Player1 ? Color.FromRgb(255, 0, 0) : Color.FromRgb(0, 0, 0)
        },
        Stroke = new SolidColorBrush {
          Color = Color.FromRgb(255, 255, 255)
        },
        StrokeThickness = 2
      };
    }
  }
}
