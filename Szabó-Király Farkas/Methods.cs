using System;
using System.Collections.Generic;

namespace Labirintus
{
    internal class Methods
    {
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

        /// <summary>
        /// Megadja, hogy hány termet tartalmaz a térkép
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Termek száma</returns>
        public static int GetRoomNumber(char[,] map)
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

        /// <summary>
        /// A kapott térkép széleit végignézve megállapítja, hogy hány kijárat van.
        /// Csak azok számítanak, amelyek nyílással rendelkeznek a széle irányába.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Az alkalmas kijáratok száma</returns>
        public static int GetSuitableEntrance(char[,] map)
        {
            int sorok = map.GetLength(0);
            int oszlopok = map.GetLength(1);
            int kijarat = 0;

            for (int y = 0; y < oszlopok; y++)
            {
                if (fel(map[0, y]))
                    kijarat++;
            }

            for (int y = 0; y < oszlopok; y++)
            {
                if (le(map[sorok - 1, y]))
                    kijarat++;
            }

            for (int x = 1; x < sorok - 1; x++)
            {
                if (balra(map[x, 0]))
                    kijarat++;
            }

            for (int x = 1; x < sorok - 1; x++)
            {
                if (jobbra(map[x, oszlopok - 1]))
                    kijarat++;
            }

            return kijarat;
        }

        /// <summary>
        /// Megnézi, hogy van-e a térképen meg nem engedett karakter?
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>true - A térkép tartalmaz szabálytalan karaktert, false - nincs benne ilyen</returns>
        public static bool IsInvalidElement(char[,] map)
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

        /// <summary>
        /// Visszaadja azoknak a járatkaraktereknek a pozícióját, amelyekhez
        /// egyetlen szomszéd pozícióból sem lehet eljutni (teljesen elszigetelt járat).
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>A pozíciók "sor_index:oszlop_index" formátumban szerepelnek a lista elemeiként</returns>
        public static List<string> GetUnavailableElements(char[,] map)
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

        /// <summary>
        /// Labirintust generál a kapott pozíciókat tartalmazó lista alapján.
        /// A lista elemei egymáshoz kapcsolódó járatok pozíciói.
        /// </summary>
        /// <param name="positionsList">"sor_index:oszlop_index" formátumban az egymáshoz kapcsolódó járatok pozícióit tartalmazó lista</param>
        /// <returns>A létrehozott labirintus térképe</returns>
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