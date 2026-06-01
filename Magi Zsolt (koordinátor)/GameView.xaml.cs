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
    public partial class GameView : UserControl
    {
        //ez alapján a kis kérdőjeles kettőspontos tuti van valami fany ahh neve hülyeséggel döntjük el, hogy mi melyik nyelv
        public bool engPerhun = true;
        public Point playerPos = new Point(0, 0);
        public Player player;
        private int chamberCounter = 0;
       

        private MediaPlayer musicPlayer = new MediaPlayer();
        private Map map;
        private int chamberCount = 0;
        public GameView(Map selectedMap)
        {
            InitializeComponent();
            map = selectedMap;

            chamberCount = map.chambers.Count;

            playerPos = determineStartPos();
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

            for (int y = 0; y <= maxY; y++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int x = 0; x <= maxX; x++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

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

            if (newX < 0 || newY < 0 ||
                newX >= maxX || newY >= maxY)
            {
                return;
            }

            Tile targetTile = map.tiles[newX, newY];

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

            if (!CheckConnections(currentTile, targetTile, dir))
                return;

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

            playerPos = new Point(newX, newY);

            playerX = newX;
            playerY = newY;

            checkChamber(map.tiles[playerX, playerY]);
            ShowAvailableDirections(map.tiles[playerX, playerY]);

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

        private void ShowAvailableDirections(Tile current)
        {
            List<string> directions = new List<string>();

            if (current.connections[0])
                directions.Add(engPerhun ? "Fel\n" : "Up\n");

            if (current.connections[1])
                directions.Add(engPerhun ? "Le\n" : "Down\n");

            if (current.connections[2])
                directions.Add(engPerhun ? "Bal\n" : "Left\n");

            if (current.connections[3])
                directions.Add(engPerhun ? "Jobb\n" : "Right\n");

            txtLepesek.Text = string.Join("", directions);
        }

        private void checkChamber(Tile current)
        {
            if (current.type == Tile.tileType.Chamber)
            {
                chamberCounter++;
                current.type = Tile.tileType.Empty;
                MessageBox.Show(engPerhun ? "Kamra megtalálva!" : "Chamber found!");
                txtKincsestermek.Text = chamberCounter.ToString() + "/" + chamberCount;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ExitGame();
        }

        private bool CheckConnections(Tile current, Tile target, Vector2 dir)
        {
            if (dir.Y == -1)
            {
                return current.connections[0] && target.connections[1];
            }

            if (dir.Y == 1)
            {
                return current.connections[1] && target.connections[0];
            }

            if (dir.X == -1)
            {
                return current.connections[2] && target.connections[3];
            }

            if (dir.X == 1)
            {
                return current.connections[3] && target.connections[2];
            }

            return false;
        }

        private Point determineStartPos()
        {
            if (map.playerPos != new Point(0, 0))
            {
                return map.playerPos;
            }
            else if (map.exits == null || map.exits.Count == 0)
                return new Point(0, 0);

            Random rnd = new Random();
            return map.exits[rnd.Next(map.exits.Count)];
        }

        private void checkExit()
        {
            if (chamberCount > chamberCounter)
            {
                MessageBox.Show(engPerhun ? "Még van fel nem fedezett kamra!" : "There are still undiscovered chambers!");
            }
            else
            {
                MessageBox.Show(engPerhun ? "Nyertél!" : "You won!");
                ExitGame();
            }
        }

        private void ExitGame()
        {
            ((MainWindow)Application.Current.MainWindow).ShowStartView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

        public static void ChangeLanguage(bool culture)
        {
            ResourceDictionary dict = new ResourceDictionary();

            switch (culture)
            {
                case true:
                    dict.Source =
                        new Uri("Languages/Hu.xaml",
                        UriKind.Relative);
                    break;

                case false:
                    dict.Source =
                        new Uri("Languages/Eng.xaml",
                        UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            engPerhun = !engPerhun;
            ChangeLanguage(engPerhun);
        }
    }
}