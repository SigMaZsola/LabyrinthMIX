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
        static char[] ervenyes = { '.', '█', '╬', '═', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔' };

        static char[] jaratKarakterek = { '╬', '═', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔' };

        static bool tartalmaz(char[] tomb, char karakter)
        {
            for (int i = 0; i < tomb.Length; i++)
            {
                if (tomb[i] == karakter)
                    return true;
            }
            return false;
        }

        static bool fel(char c)
        {
            return c == '║' || c == '╚' || c == '╝' || c == '╠' || c == '╣' || c == '╩' || c == '╬';
        }

        static bool le(char c)
        {
            return c == '║' || c == '╔' || c == '╗' || c == '╠' || c == '╣' || c == '╦' || c == '╬';
        }

        static bool balra(char c)
        {
            return c == '═' || c == '╗' || c == '╝' || c == '╣' || c == '╦' || c == '╩' || c == '╬';
        }

        static bool jobbra(char c)
        {
            return c == '═' || c == '╔' || c == '╚' || c == '╠' || c == '╦' || c == '╩' || c == '╬';
        }
        public int GetRoomNumber()
        {
            int szoba = 0;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == '█')
                    {
                        szoba++;
                    }
                }
            }
            return szoba;
        }
        public int GetSuitableEntrance()
        {
            int sorok = map.GetLength(0);
            int oszlopok = map.GetLength(1);
            int kijarat = 0;

            for (int y = 1; y < oszlopok - 1; y++)
                if (fel(map[0, y])) kijarat++;

            for (int y = 1; y < oszlopok - 1; y++)
                if (le(map[sorok - 1, y])) kijarat++;

            for (int x = 1; x < sorok - 1; x++)
                if (balra(map[x, 0])) kijarat++;

            for (int x = 1; x < sorok - 1; x++)
                if (jobbra(map[x, oszlopok - 1])) kijarat++;

            if (fel(map[0, 0]) || balra(map[0, 0])) kijarat++;

            if (fel(map[0, oszlopok - 1]) || jobbra(map[0, oszlopok - 1])) kijarat++;

            if (le(map[sorok - 1, 0]) || balra(map[sorok - 1, 0])) kijarat++;

            if (le(map[sorok - 1, oszlopok - 1]) || jobbra(map[sorok - 1, oszlopok - 1])) kijarat++;

            return kijarat;
        }
        public bool IsInvalidElement()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (!tartalmaz(ervenyes, map[x, y]))
                        return true;
                }
            }
            return false;
        }
        public List<string> GetUnavailableElements()
        {
            List<string> eredmeny = new List<string>();
            int sorok = map.GetLength(0);
            int oszlopok = map.GetLength(1);

            for (int x = 0; x < sorok; x++)
            {
                for (int y = 0; y < oszlopok; y++)
                {
                    if (!tartalmaz(jaratKarakterek, map[x, y]))
                        continue;

                    bool vanSzomszed = false;

                    if (x - 1 >= 0)
                    {
                        if (tartalmaz(jaratKarakterek, map[x - 1, y]) || map[x - 1, y] == '█')
                            vanSzomszed = true;
                    }
                    if (x + 1 < sorok)
                    {
                        if (tartalmaz(jaratKarakterek, map[x + 1, y]) || map[x + 1, y] == '█')
                            vanSzomszed = true;
                    }
                    if (y - 1 >= 0)
                    {
                        if (tartalmaz(jaratKarakterek, map[x, y - 1]) || map[x, y - 1] == '█')
                            vanSzomszed = true;
                    }
                    if (y + 1 < oszlopok)
                    {
                        if (tartalmaz(jaratKarakterek, map[x, y + 1]) || map[x, y + 1] == '█')
                            vanSzomszed = true;
                    }

                    if (!vanSzomszed)
                    {
                        eredmeny.Add(x + ":" + y);
                    }
                }
            }
            return eredmeny;
        }
        public static char[,] GenerateLabyrinth(List<string> positionsList)
        {
            if (positionsList == null || positionsList.Count == 0)
                return null;

            int[] sorok = new int[positionsList.Count];
            int[] oszlopok = new int[positionsList.Count];

            for (int i = 0; i < positionsList.Count; i++)
            {
                string[] reszek = positionsList[i].Split(':');
                sorok[i] = int.Parse(reszek[0]);
                oszlopok[i] = int.Parse(reszek[1]);
            }

            int maxSor = 0;
            int maxOszlop = 0;
            for (int i = 0; i < sorok.Length; i++)
            {
                if (sorok[i] > maxSor) maxSor = sorok[i];
                if (oszlopok[i] > maxOszlop) maxOszlop = oszlopok[i];
            }

            char[,] map = new char[maxSor + 1, maxOszlop + 1];

            for (int x = 0; x <= maxSor; x++)
                for (int y = 0; y <= maxOszlop; y++)
                    map[x, y] = '.';

            for (int i = 0; i < sorok.Length; i++)
            {
                int sor = sorok[i];
                int oszlop = oszlopok[i];

                bool fel = false, le = false, bal = false, jobb = false;

                for (int j = 0; j < sorok.Length; j++)
                {
                    if (sorok[j] == sor - 1 && oszlopok[j] == oszlop) fel = true;
                    if (sorok[j] == sor + 1 && oszlopok[j] == oszlop) le = true;
                    if (sorok[j] == sor && oszlopok[j] == oszlop - 1) bal = true;
                    if (sorok[j] == sor && oszlopok[j] == oszlop + 1) jobb = true;
                }

                map[sor, oszlop] = KarakterKivalasztas(fel, le, bal, jobb);
            }

            return map;
        }

        static char KarakterKivalasztas(bool fel, bool le, bool bal, bool jobb)
        {
            if (fel && le && bal && jobb) return '╬';
            if (fel && le && !bal && !jobb) return '║';
            if (!fel && !le && bal && jobb) return '═';
            if (fel && !le && bal && jobb) return '╩';
            if (!fel && le && bal && jobb) return '╦';
            if (fel && le && !bal && jobb) return '╠';
            if (fel && le && bal && !jobb) return '╣';
            if (!fel && le && !bal && jobb) return '╔';
            if (!fel && le && bal && !jobb) return '╗';
            if (fel && !le && !bal && jobb) return '╚';
            if (fel && !le && bal && !jobb) return '╝';
            if (bal || jobb) return '═';
            return '║';
        }
    }
}
