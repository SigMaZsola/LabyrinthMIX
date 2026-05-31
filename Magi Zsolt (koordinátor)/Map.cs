using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirintus
{
    public class Map
    {
        public Tile[,] tiles;
        public string name;
        //list a kijáratok koordinátáival
        public List<System.Windows.Point> exits = new List<System.Windows.Point>();
        //list a chamberek koordinátával
        public List<System.Windows.Point> chambers = new List<System.Windows.Point>();
        public Map(string name)
        {
            this.name = name;

            tiles = readTileMap();
            GetSuitableEntrance();
            GetRoomNumber();
        }

        private Tile[,] readTileMap()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != true)
                return null;

            string[] sorok = File.ReadAllLines(ofd.FileName);

            int magassag = sorok.Length;
            int szelesseg = sorok[0].Length;

            Tile[,] readTiles = new Tile[szelesseg, magassag];

            for (int y = 0; y < sorok.Length; y++)
            {
                string sor = sorok[y];

                for (int x = 0; x < sor.Length; x++)
                {
                    char c = sor[x];

                    if (c == '.')
                        continue;

                    Tile.tileType type;

                    switch (c)
                    {
                        case '█':
                            type = Tile.tileType.Chamber;
                            break;

                        case '╬':
                            type = Tile.tileType.Cross;
                            break;

                        case '═':
                            type = Tile.tileType.Horizontal;
                            break;

                        case '║':
                            type = Tile.tileType.Vertical;
                            break;

                        case '╩':
                            type = Tile.tileType.UpT;
                            break;

                        case '╦':
                            type = Tile.tileType.T;
                            break;

                        case '╠':
                            type = Tile.tileType.RightT;
                            break;

                        case '╣':
                            type = Tile.tileType.LeftT;
                            break;

                        case '╗':
                            type = Tile.tileType.TopRightC;
                            break;

                        case '╔':
                            type = Tile.tileType.TopLeftC;
                            break;

                        case '╝':
                            type = Tile.tileType.BottomRightC;
                            break;

                        case '╚':
                            type = Tile.tileType.BottomLeftC;
                            break;

                        default:
                            continue;
                    }

                    Tile tile = new Tile(x, y, type, c);

                    readTiles[x, y] = tile;
                }
            }

            return readTiles;
        }

        //Farkas által megírt algoritmusok implelemntálása
        
        //Kijáratok
        public void GetSuitableEntrance()
        {
            exits.Clear();

            int szelesseg = tiles.GetLength(0);
            int magassag = tiles.GetLength(1);

            // Bal széle: balra nyitott csempék
            for (int y = 0; y < magassag; y++)
            {
                Tile t = tiles[0, y];
                if (t != null && t.connections[2])
                    exits.Add(new System.Windows.Point(0, y));
            }

            // Jobb széle: jobbra nyitott csempék
            for (int y = 0; y < magassag; y++)
            {
                Tile t = tiles[szelesseg - 1, y];
                if (t != null && t.connections[3])
                    exits.Add(new System.Windows.Point(szelesseg-1, y));
            }

            // Felső széle: felfelé nyitott csempék
            for (int x = 0; x < szelesseg; x++)
            {
                Tile t = tiles[x, 0];
                if (t != null && t.connections[0])
                    exits.Add(new System.Windows.Point(x, 0));
            }

            // Alsó széle: lefelé nyitott csempék
            for (int x = 0; x < szelesseg; x++)
            {
                Tile t = tiles[x, magassag - 1];
                if (t != null && t.connections[1])
                    exits.Add(new System.Windows.Point(x, magassag-1));
            }
        }

        //Kincsestermek
        public void GetRoomNumber()
        {
            chambers.Clear();

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Tile t = tiles[x, y];
                    if (t != null && t.type == Tile.tileType.Chamber)
                        chambers.Add(new System.Windows.Point(x, y));
                }
            }
        }
    }
}
