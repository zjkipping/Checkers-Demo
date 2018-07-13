using System.Windows;
using System.Collections.Generic;

namespace FinalProject {
  public static class Constants {
    public static Point NORTH = new Point(0, 1);
    public static Point NORTH_EAST = new Point(1, 1);
    public static Point EAST = new Point(1, 0);
    public static Point SOUTH_EAST = new Point(1, -1);
    public static Point SOUTH = new Point(0, -1);
    public static Point SOUTH_WEST = new Point(-1, -1);
    public static Point WEST = new Point(-1, 0);
    public static Point NORTH_WEST = new Point(-1, 1);

    public static List<Point> CARDINALS = new List<Point>(new Point[] { NORTH, NORTH_EAST, EAST, SOUTH_EAST, SOUTH, SOUTH_WEST, WEST, NORTH_WEST });

    public static string[] LetterPositions = { "A", "B", "C", "D", "E", "F", "G", "H"};
  }
}
