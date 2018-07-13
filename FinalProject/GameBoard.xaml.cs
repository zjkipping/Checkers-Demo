using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinalProject {
  public partial class GameBoard : Window {
    Board board;

    public GameBoard() {
      InitializeComponent();
    }

    private void GameScreen_Loaded(object sender, RoutedEventArgs e) {
      board = new Board(GameScreen);
    }
  }
}
