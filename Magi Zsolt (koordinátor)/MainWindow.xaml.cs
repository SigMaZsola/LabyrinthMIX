using System.IO;
using System.Media;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Labirintus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Map> Maps { get; private set; } = new List<Map>();
        public MainWindow()
        {
            InitializeComponent();

            ShowStartView();
        }

        public void ShowStartView()
        {
            MainContent.Content = new StartView();
        }

        public void ShowGameView(Map map)
        {
            MainContent.Content = new GameView(map);
        }
    }
}