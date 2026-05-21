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
using System.IO;


namespace Labirintus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int playerX = 0;
        public int playerY = 0;
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
                lbl.Background = Brushes.LightGray;
                //Font
                //lbl.FontFamily = new FontFamily("Consolas");
                Grid.SetColumn(lbl, tile.posX);
                Grid.SetRow(lbl, tile.posY);

                GameGrid.Children.Add(lbl);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            MediaPlayer player = new MediaPlayer();

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "sound.mp3");
            

            player.Open(new Uri(path));


            var tab = (sender as TabControl).SelectedItem as TabItem;

            if (tab.Header.ToString() == "Nehéz mód")
            {
                player.Play();
            }
            else if (tab.Header.ToString() != "Nehéz mód")
            {
                player.Stop();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                MovePlayer(new Vector2(0, -1));
            }
            if (e.Key == Key.A)
            {
                MovePlayer(new Vector2(-1, 0));

            }
            if (e.Key == Key.S)
            {
                MovePlayer(new Vector2(0, 1));

            }
            if (e.Key == Key.D)
            {
                MovePlayer(new Vector2(1, 0));

            }
        }
        private void MovePlayer(Vector2 dir)
        {

            Player player = new Player('P', playerX, playerY);
            playerX += (int)dir.X;
            playerY += (int)dir.Y;
            player.posX = playerX;
            player.posY = playerY;
            foreach (UIElement child in GameGrid.Children)
            {
                if (child is Label label)
                {
                    int x = Grid.GetColumn(label);
                    int y = Grid.GetRow(label);

                    if (x == playerX && y == playerY)
                    {
                        label.Content = "P";
                    }
                    else
                    {
                        label.Content = "";
                    }
                }
            }
        }


        private void CheckNeighbours() {
            Vector2 playerPos = new Vector2(playerX, playerY);
            
        }
    }
}