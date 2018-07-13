using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FinalProject {
  public class Tile: BoardObject {
    public Tile(Point position) {
      Position = position;
      Shape = new Rectangle {
        Width = 100,
        Height = 100,
        Fill = new SolidColorBrush {
          Color = (position.X + position.Y) % 2 == 0 ? Color.FromRgb(255, 0, 0) : Color.FromRgb(45, 45, 45)
        },
        StrokeThickness = 2
      };
    }
  }
}
