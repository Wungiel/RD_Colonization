using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using System;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    internal class MapGenerator
    {
        public Tile[,] generate(int size)
        {
            double[,] temp = new double[size, size];
            Tile[,] tileTemp = new Tile[size, size];

            for (double i = 0; i < size; i++)
            {
                for (double y =0; y < size; y++)
                {
                    temp[(int)i, (int)y] = (Perlin.OctavePerlin(i / size, y / size, 0, 3, 1));
                }
            }

            temp = normalizeArray(temp);

            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int y = 0; y < temp.GetLength(0); y++)
                {
                    string tileKey = null;
                    if (temp[i, y] > 0.7)
                        tileKey = mountainString;
                    else if (temp[i, y] > 0.4)
                        tileKey = grassString;
                    else
                        tileKey = waterString;
                    tileTemp[i, y] = new Tile(MapManager.getTileType(tileKey), new Point(i, y));
                }
            }

            return tileTemp;
        }

        private double[,] normalizeArray(double[,] temp)
        {
            double min = temp[0, 0];
            double max = temp[0, 0];
            foreach (double i in temp)
            {
                if (i > max)
                    max = i;
                else if (i < min)
                    min = i;
            }

            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int y = 0; y < temp.GetLength(0); y++)
                {
                    temp[i, y] = (temp[i, y] - min) / (max - min);
                }
            }

            return temp;
        }
    }
}