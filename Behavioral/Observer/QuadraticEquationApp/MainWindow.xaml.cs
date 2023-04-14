using System.Windows;

namespace QuadraticEquationApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public View View { get; set; } = new(new QuadraticEquationSolver());

    public MainWindow()
    {
      InitializeComponent();
    }
  }
}
