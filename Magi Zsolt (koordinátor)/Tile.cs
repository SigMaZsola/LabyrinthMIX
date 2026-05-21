using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirintus
{
    internal class Tile
    {
        public enum tileType
        {
            Empty,
            Chamber,
            Cross,
            Horizontal,
            T,
            UpT,
            RightT,
            LeftT,
            Vertical,
            TopRightC,
            TopLeftC,
            BottomRightC,
            BottomLeftC

        }
        public char icon;
        public int posX;
        public int posY;
        public tileType type;
        public List<bool> connections = new List<bool>(new bool[4]); // Fel, Le, Balra, Jobbra


        public Tile(int posX, int posY, tileType type, char icon)
        {
            this.posX = posX;
            this.posY = posY;
            this.type = type;
            connections = DetermineConnections(type);
            this.icon = icon;
        }

        private List<bool> DetermineConnections(tileType type)
        {
            List<bool> calculatedConnections = new List<bool>(new bool[4]); // Fel, Le, Balra, Jobbra
            switch (type)
            {
                case tileType.Empty:
                    // Nincs kapcsolódás
                    calculatedConnections = new List<bool> { false, false, false, false };
                    break;
                case tileType.Cross:
                    // MIndenhová kapcsolódik
                    calculatedConnections = new List<bool> { true, true, true, true };
                    break;
                case tileType.Chamber:
                    // MIndenhová kapcsolódik
                    calculatedConnections = new List<bool> { true, true, true, true };
                    break;
                case tileType.Horizontal:
                    // Balra és jobbra kapcsolódik
                    calculatedConnections = new List<bool> { false, false, true, true };

                    break;
                case tileType.T:
                    // Balra és jobbra kapcsolódik, de nem lefelé
                    calculatedConnections = new List<bool> { false, true, true, true };

                    break;
                case tileType.UpT:
                    //  Balra, jobbra és felfelé kapcsolódik, de nem lefelé
                    calculatedConnections = new List<bool> { true, false, true, true };

                    break;
                case tileType.RightT:
                    // Fel, le és jobbra kapcsolódik, de nem balra
                    calculatedConnections = new List<bool> { true, true, false, true };

                    break;
                case tileType.LeftT:
                    // Balra, fel és le kapcsolódik, de nem jobbra
                    calculatedConnections = new List<bool> { true, true, true, false };

                    break;
                case tileType.Vertical:
                    // Fel és le kapcsolódik
                    calculatedConnections = new List<bool> { true, true, false, false };

                    break;
                case tileType.TopRightC:
                    // Fel és jobbra kapcsolódik
                    calculatedConnections = new List<bool> { true, false, false, true };

                    break;
                case tileType.TopLeftC:
                    // Fel és balra kapcsolódik
                    calculatedConnections = new List<bool> { true, false, true, false };

                    break;
                case tileType.BottomRightC:
                    // Le és jobbra kapcsolódik
                    calculatedConnections = new List<bool> { false, true, true, false };

                    break;
                case tileType.BottomLeftC:
                    // Le és balra kapcsolódik
                    calculatedConnections = new List<bool> { false, true, false, true };

                    break;
            }
            Console.WriteLine("Kapcsolatok meghatározva");
            return calculatedConnections;
        }


    }
}
