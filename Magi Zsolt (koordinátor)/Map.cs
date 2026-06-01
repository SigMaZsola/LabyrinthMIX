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

        public List<System.Windows.Point> exits = new();
        public List<System.Windows.Point> chambers = new();

        public System.Windows.Point playerPos;
        public int chamberCounter;

        public Map(string name, Tile[,] tiles)
        {
            this.name = name;
            this.tiles = tiles;

            GetSuitableEntrance();
            GetRoomNumber();
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

        //A karaktereket átalakítja tileokká
        


        

    }
}
