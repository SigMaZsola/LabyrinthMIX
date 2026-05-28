using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MapEditor
{
    internal class Labyrinth
    {
        private char[,] map;
        public int row;
        public int col;

        public Labyrinth(int row, int col)
        {
            this.row = row;
            this.col = col;
            map = new char[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    map[i, j] = '.';
                }
            }
        }

        public void AddPath(string path, int r, int c)
        {
            map[r, c] = path == "" ? '.' : Convert.ToChar(path);
        }

        public char[,] Map { get => map; }

        public List<string> ToStringRows()
        {
            List<string> final = new List<string>();

            for (int i = 0; i < row; i++)
            {
                string row = "";

                for (int j = 0; j < col; j++)
                {
                    row += map[i, j];
                }
                final.Add(row);
            }
            return final;
        } 
    }
}
