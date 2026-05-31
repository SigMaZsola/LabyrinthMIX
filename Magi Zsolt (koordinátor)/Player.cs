using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirintus
{
    public class Player
    {
        public Point position;
        public char icon = 'P';

        public Player(char icon, Point position)
        {
            this.position = position;
            this.icon = icon;

        }
            
    }
}
