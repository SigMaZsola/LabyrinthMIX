using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
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
using Labirintus;
using Microsoft.Win32;

namespace Faszomat
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public Point playerPos = new Point(0, 0);
        public Player player;
        private int chamberCounter = 0;
        private bool didGetOut = false;

        private MediaPlayer musicPlayer = new MediaPlayer();
        private Map map;
        private int chamberCount = 0;
        public GameView(Map selectedMap)
        {
            InitializeComponent();
            map = selectedMap;

            chamberCount = map.chambers.Count;
           
            //eldöntjük, hogy melyik csempén kezd a player
            playerPos = determineStartPos();
            //player létrehozása
            player = new Player('P', playerPos);

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

                if (tile.posX == playerPos.X && tile.posY == playerPos.Y)
                {
                    lbl.Content = "P";
                }
                else
                {
                    lbl.Content = tile.icon;
                }
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



            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "sound.mp3");


            musicPlayer.Open(new Uri(path));


            var tab = (sender as TabControl).SelectedItem as TabItem;

            if (tab.Header.ToString() == "Nehéz mód")
            {
                musicPlayer.Play();
            }
            else if (tab.Header.ToString() != "Nehéz mód")
            {
                musicPlayer.Stop();
            }
        }

        //Inputok érzékelése
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this);
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
            int playerX = (int)playerPos.X;
            int playerY = (int)playerPos.Y;


            int newX = playerX + (int)dir.X;
            int newY = playerY + (int)dir.Y;

            int maxX = map.tiles.GetLength(0);
            int maxY = map.tiles.GetLength(1);

            // kilépés esetén NEM lépünk tovább
            if (newX < 0 || newY < 0 ||
                newX >= maxX || newY >= maxY)
            {
                return;
            }

            Tile targetTile = map.tiles[newX, newY];

            // EXIT CHECK CSAK AKKOR
            foreach (Point exit in map.exits)
            {
                

                if (targetTile == null)
                    return;
                if (targetTile.posX == exit.X && targetTile.posY == exit.Y)
                {
                    checkExit();
                    break;
                }
            }

            Tile currentTile = map.tiles[playerX, playerY];


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

            playerPos = new Point(newX, newY);

            playerX = newX;
            playerY = newY;
            checkChamber(map.tiles[playerX, playerY]);
            ShowAvailableDirections(map.tiles[playerX, playerY]);
            // új P kirajzolása
            foreach (UIElement child in GameGrid.Children)
            {
                if (child is Label label)
                {
                    int x = Grid.GetColumn(label);
                    int y = Grid.GetRow(label);

                    if (x == playerX && y == playerY)
                    {
                        label.Content = player.icon;
                    }
                }
            }
        }

        //szomszédok megnézése
        //private List<Tile> CheckNeighbours()
        //{
        //    //Játékos pozíciója
        //    Point playerPos = new Point(playerX, playerY);
        //    //javítva vector2-ről pontokra (irányok)
        //    Point[] directions =
        //    {
        //        new Point(0, -1),
        //        new Point(-1, 0),
        //        new Point(0, 1),
        //        new Point(1, 0)
        //    };
        //    //szomszédokat ebben tároljuk

        //    List<Tile> neighbours = new List<Tile>();
        //    //négy irányban megnézzük, hogy a player jelenlegi pozíciója mellett milyen csempék vannak
        //    foreach (Point dir in directions)
        //    {
        //        int nx = (int)(playerPos.X + dir.X);
        //        int ny = (int)(playerPos.Y + dir.Y);
        //        //kívülre mutató koordináták kizárása
        //        bool outside =
        //            nx < 0 ||
        //            ny < 0 ||
        //            nx >= map.tiles.GetLength(0) ||
        //            ny >= map.tiles.GetLength(1);

        //        if (outside)
        //            continue;


        //        Tile neighbour = map.tiles[nx, ny];

        //        if (neighbour != null)
        //        {
        //            neighbours.Add(neighbour);
        //        }
        //    }

        //    return neighbours;
        //}

        //MInden irányból elérhető csempék megnézése
        private void ShowAvailableDirections(Tile current)
        {
            List<string> directions = new List<string>();

            if (current.connections[0])
                directions.Add("Fel\n");

            if (current.connections[1])
                directions.Add("Le\n");

            if (current.connections[2])
                directions.Add("Bal\n");

            if (current.connections[3])
                directions.Add("Jobb\n");

            txtLepesek.Text = string.Join("", directions);
        }
        //megnézi jártunk már-e ebben a kincseskamrában
        private void checkChamber(Tile current)
        {
            if (current.type == Tile.tileType.Chamber)
            {
                chamberCounter++;
                current.type = Tile.tileType.Empty;
                MessageBox.Show("Kamra megtalálva!");
                txtKincsestermek.Text = chamberCounter.ToString() + "/" + chamberCount;
            }
        }


        //Vissszalépés a startmenüre
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ExitGame();
        }
        //egy bizonyos csempe és a jeléenlegi csempe kapcsolat leellenőrzése
        private bool CheckConnections(Tile current, Tile target, Vector2 dir)
        {
            // fel
            if (dir.Y == -1)
            {

                return current.connections[0] && target.connections[1];
            }

            // le
            if (dir.Y == 1)
            {

                return current.connections[1] && target.connections[0];
            }

            // bal
            if (dir.X == -1)
            {

                return current.connections[2] && target.connections[3];
            }

            // jobb
            if (dir.X == 1)
            {

                return current.connections[3] && target.connections[2];
            }

            return false;
        }



        private Point determineStartPos()
        {
            if (map.playerPos != new Point(0,0)) {
                return map.playerPos;
            }
            else if (map.exits == null || map.exits.Count == 0)
                return new Point(0, 0);

            Random rnd = new Random();
            return map.exits[rnd.Next(map.exits.Count)];
        }

        //Nyertünk-e?
        private void checkExit() {
            if (chamberCount > chamberCounter)
            {
                MessageBox.Show("Még van fel nem fedezett kamra!");
            }
            else {
                MessageBox.Show("Nyertél!");
                ExitGame();
            }
            
        }
        private void ExitGame() {
            ((MainWindow)Application.Current.MainWindow).ShowStartView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Adatok és pálya elmentése
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Save File (*.SAV)|*.SAV",
                DefaultExt = ".SAV"
            };

            if (sfd.ShowDialog() != true)
                return;

            int width = map.tiles.GetLength(0);
            int height = map.tiles.GetLength(1);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("MAP");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tile tile = map.tiles[x, y];
                    sb.Append(tile == null ? '.' : tile.icon);
                }
                sb.AppendLine();
            }

            sb.AppendLine("ENDMAP");

            sb.AppendLine("PLAYER");
            sb.AppendLine($"{playerPos.X};{playerPos.Y}");
            sb.AppendLine("ENDPLAYER");

            sb.AppendLine("CHAMBERS");
            sb.AppendLine(chamberCounter.ToString());
            sb.AppendLine("ENDCHAMBERS");

            System.IO.File.WriteAllText(sfd.FileName, sb.ToString());
        }
    }

 }

