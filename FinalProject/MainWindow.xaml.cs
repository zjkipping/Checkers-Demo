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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProject {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    public void GoToBoard() {
      GameBoard gameBoard = new GameBoard();
      Application.Current.MainWindow = gameBoard;
      Close();
      gameBoard.Show();
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      GoToBoard();
    }
  }
}
