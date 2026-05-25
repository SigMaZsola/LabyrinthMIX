using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirintus
{
    internal class Map
    {
        public Tile[,] tiles;

        public Map()
        {
 
            tiles = readTileMap();
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
                            type = Tile.tileType.T;
                            break;

                        case '╦':
                            type = Tile.tileType.UpT;
                            break;

                        case '╠':
                            type = Tile.tileType.RightT;
                            break;

                        case '╣':
                            type = Tile.tileType.LeftT;
                            break;

                        case '╗':
                            type = Tile.tileType.TopLeftC;
                            break;

                        case '╔':
                            type = Tile.tileType.TopRightC;
                            break;

                        case '╝':
                            type = Tile.tileType.BottomLeftC;
                            break;

                        case '╚':
                            type = Tile.tileType.BottomRightC;
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
    }
}
