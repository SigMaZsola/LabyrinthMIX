using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirintus
{
    class Player
    {
        public int posX;
        public int posY;
        char icon = 'P';

        public Player(char icon, int posX = 0, int posY = 0)
        {
            this.posX = posX;
            this.posY = posY;
            this.icon = icon;

        }
            
    }
}
