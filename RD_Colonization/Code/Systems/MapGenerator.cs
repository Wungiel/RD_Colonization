using RD_Colonization.Code.Data;
using System;
using System.Diagnostics;

namespace RD_Colonization.Code.Managers
{
    internal class MapGenerator
    {
        public MapData generate(int size)
        {
            double[,] temp = new double[size, size];

            for (double i = 0; i < size; i++)
            {
                for (double y =0; y < size; y++)
                {
                    temp[(int)i, (int)y] = (Perlin.OctavePerlin(i / size, y / size, 0, 3, 1));
                }
            }



            temp = normalizeArray(temp);



            return null;
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