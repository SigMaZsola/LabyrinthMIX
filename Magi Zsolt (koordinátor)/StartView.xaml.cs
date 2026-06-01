using Labirintus;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Labirintus;

public partial class StartView : UserControl
{
    public bool engPerhun = true;

    private MainWindow Main => (MainWindow)Application.Current.MainWindow;

    public StartView()
    {
        InitializeComponent();
        RefreshList();
    }

    private void RefreshList()
    {
        lbSaves.Items.Clear();

        foreach (var map in Main.Maps)
        {
            lbSaves.Items.Add(map.name);
        }
    }

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        if (lbSaves.SelectedIndex == -1)
        {
            MessageBox.Show("Válassz térképet!");
            return;
        }

        Map selectedMap = Main.Maps[lbSaves.SelectedIndex];
        Main.ShowGameView(selectedMap);
    }

    // ÚJ PÁLYA HOZZÁADÁSA
    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNameGiver.Text))
        {
            MessageBox.Show("Adj nevet a térképnek!");
            return;
        }

        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Map or Save (*.txt;*.sav)|*.txt;*.sav";

        if (ofd.ShowDialog() != true)
            return;

        string[] lines = File.ReadAllLines(ofd.FileName);

        Map map;

        bool isSave =
            lines.Contains("MAP") &&
            lines.Contains("ENDMAP") &&
            lines.Contains("PLAYER");

        if (isSave)
        {
            map = LoadSave(lines, txtNameGiver.Text);
        }
        else
        {
            Tile[,] tiles = LoadMapFromTxt(lines);
            map = new Map(txtNameGiver.Text, tiles);
        }

        Main.Maps.Add(map);
        RefreshList();
    }

    // MENTÉS BETÖLTÉSE
    private void Load_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Save File (*.sav)|*.sav";

        if (ofd.ShowDialog() != true)
            return;

        string[] lines = File.ReadAllLines(ofd.FileName);

        Map map = LoadSave(
            lines,
            Path.GetFileNameWithoutExtension(ofd.FileName)
        );

        Main.Maps.Add(map);
        RefreshList();
    }

    private Map LoadSave(string[] lines, string mapName)
    {
        List<string> mapLines = new List<string>();

        Point playerPos = new Point(0, 0);
        int chamberCounter = 0;

        bool readingMap = false;
        bool readingPlayer = false;
        bool readingChambers = false;

        foreach (string line in lines)
        {
            if (line == "MAP")
            {
                readingMap = true;
                continue;
            }

            if (line == "ENDMAP")
            {
                readingMap = false;
                continue;
            }

            if (line == "PLAYER")
            {
                readingPlayer = true;
                continue;
            }

            if (line == "ENDPLAYER")
            {
                readingPlayer = false;
                continue;
            }

            if (line == "CHAMBERS")
            {
                readingChambers = true;
                continue;
            }

            if (line == "ENDCHAMBERS")
            {
                readingChambers = false;
                continue;
            }

            if (readingMap)
            {
                mapLines.Add(line);
                continue;
            }

            if (readingPlayer)
            {
                string[] parts = line.Split(';');

                if (parts.Length == 2)
                {
                    playerPos = new Point(
                        int.Parse(parts[0]),
                        int.Parse(parts[1]));
                }

                continue;
            }

            if (readingChambers)
            {
                int.TryParse(line, out chamberCounter);
            }
        }

        Tile[,] tiles = LoadMapFromTxt(mapLines.ToArray());

        Map map = new Map(mapName, tiles);

        map.playerPos = playerPos;
        map.chamberCounter = chamberCounter;

        return map;
    }

    private Tile[,] LoadMapFromTxt(string[] lines)
    {
        int height = lines.Length;
        int width = lines.Max(x => x.Length);

        Tile[,] tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                tiles[x, y] = CharToTile(lines[y][x], x, y);
            }
        }

        return tiles;
    }

    private Tile CharToTile(char c, int x, int y)
    {
        switch (c)
        {
            case '║':
                return new Tile(x, y, Tile.tileType.Vertical, '║');

            case '═':
                return new Tile(x, y, Tile.tileType.Horizontal, '═');

            case '╔':
                return new Tile(x, y, Tile.tileType.TopLeftC, '╔');

            case '╗':
                return new Tile(x, y, Tile.tileType.TopRightC, '╗');

            case '╚':
                return new Tile(x, y, Tile.tileType.BottomLeftC, '╚');

            case '╝':
                return new Tile(x, y, Tile.tileType.BottomRightC, '╝');

            case '╠':
                return new Tile(x, y, Tile.tileType.RightT, '╠');

            case '╣':
                return new Tile(x, y, Tile.tileType.LeftT, '╣');

            case '╦':
                return new Tile(x, y, Tile.tileType.T, '╦');

            case '╩':
                return new Tile(x, y, Tile.tileType.UpT, '╩');

            case '╬':
                return new Tile(x, y, Tile.tileType.Cross, '╬');

            case '█':
                return new Tile(x, y, Tile.tileType.Chamber, '█');

            case '.':
            case ' ':
                return null;

            default:
                return null;
        }
    }

        public static void ChangeLanguage(bool culture){
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