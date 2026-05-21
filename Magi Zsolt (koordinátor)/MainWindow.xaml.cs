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
        public MainWindow()
        {
            InitializeComponent();
                Map map = new Map();
            DrawMap(map.tiles);

        }
        private void DrawMap(List<Tile> tiles)
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            int maxX = tiles.Max(t => t.posX);
            int maxY = tiles.Max(t => t.posY);

            //Annyi sor és oszlop, amennyi a legnagyobb X és Y koordinátájú csempéhez kell
            for (int y = 0; y <= maxY; y++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }

 
            for (int x = 0; x <= maxX; x++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //Bepozíciónálás
            foreach (Tile tile in tiles)
            {
                Label lbl = new Label();

                lbl.Content = tile.icon.ToString();
                lbl.FontSize = 32;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;

                Grid.SetColumn(lbl, tile.posX);
                Grid.SetRow(lbl, tile.posY);

                GameGrid.Children.Add(lbl);
            }
        }
    }
}