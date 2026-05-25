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
        public int playerX = 5;
        public int playerY = 0;
        private Map map = new Map();
        public MainWindow()
        {
            InitializeComponent();

            DrawMap(map.tiles);


        }
        private void DrawMap(Tile[,] tiles)
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            int maxX = tiles.GetLength(0);
            int maxY = tiles.GetLength(1);

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
                if (tile == null)
                    continue;

                Label lbl = new Label();

                lbl.Content = tile.icon.ToString();
                lbl.FontSize = 32;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                lbl.Background = Brushes.LightGray;

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
            int newX = playerX + (int)dir.X;
            int newY = playerY + (int)dir.Y;

            // pályán belül maradunk?
            if (newX < 0 || newY < 0 ||
                newX >= map.tiles.GetLength(0) ||
                newY >= map.tiles.GetLength(1))
            {
                return;
            }

            Tile currentTile = map.tiles[playerX, playerY];
            Tile targetTile = map.tiles[newX, newY];

            if (targetTile == null)
                return;

            // kapcsolat ellenőrzése
            if (!CheckConnections(currentTile, targetTile, dir))
                return;

            // régi P törlése
            foreach (UIElement child in GameGrid.Children)
            {
                if (child is Label label)
                {
                    int x = Grid.GetColumn(label);
                    int y = Grid.GetRow(label);

                    if (x == playerX && y == playerY)
                    {
                        label.Content = currentTile.icon.ToString();
                    }
                }
            }

            // új pozíció
            playerX = newX;
            playerY = newY;

            // új P kirajzolása
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
                }
            }
        }

        //szomszédok megnézése
        private List<Tile> CheckNeighbours()
        {
            //Játékos pozíciója
            Point playerPos = new Point(playerX, playerY);
            //javítva vector2-ről pontokra (irányok)
            Point[] directions =
            {
                new Point(0, -1),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, 0)
            };
            //szomszédokat ebben tároljuk

            List<Tile> neighbours = new List<Tile>();
            //négy irányban megnézzük, hogy a player jelenlegi pozíciója mellett milyen csempék vannak
            foreach (Point dir in directions)
            {
                int nx = (int)(playerPos.X + dir.X);
                int ny = (int)(playerPos.Y + dir.Y);
                //kívülre mutató koordináták kizárása
                bool outside =
                    nx < 0 ||
                    ny < 0 ||
                    nx >= map.tiles.GetLength(0) ||
                    ny >= map.tiles.GetLength(1);

                if (outside)
                    continue;


                Tile neighbour = map.tiles[nx, ny];

                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }

        //egy bizonyos csempe és a jeléenlegi csempe kapcsolat leellenőrzése
        private bool CheckConnections(Tile current, Tile target, Vector2 dir)
        {
            // fel
            if (dir.Y == -1)
            {
                return current.connections[1] && target.connections[0];
            }

            // bal
            if (dir.X == -1)
            {
                return current.connections[3] && target.connections[2];
            }

            // le
            if (dir.Y == 1)
            {
                return current.connections[0] && target.connections[1];
            }

            // jobb
            if (dir.X == 1)
            {
                return current.connections[2] && target.connections[3];
            }

            return false;
        }
    }
}