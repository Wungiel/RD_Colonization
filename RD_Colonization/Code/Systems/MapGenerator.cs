using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class MapGenerator
    {
        public Tile[,] generate(int size)
        {
            double[,] temp = new double[size, size];
            Tile[,] tileTemp = new Tile[size, size];

            Random random = new Random();
            Double persistence = random.NextDouble() * 5;
            int octaves = random.Next(1, 4);

            for (double i = 0; i < size; i++)
            {
                for (double y = 0; y < size; y++)
                {
                    temp[(int)i, (int)y] = (Perlin.OctavePerlin(i / size, y / size, 0, octaves, persistence));
                }
            }

            temp = normalizeArray(temp);

            for (int i = 0; i < size; i++)
            {
                for (int y = 0; y < size; y++)
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

            List<int> availableValuesX = new List<int>();
            List<int> availableValuesY = new List<int>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    prepareAvailableValues(size, availableValuesX, availableValuesY, i, j);
                    addNeighbours(availableValuesX, availableValuesY, i, j, tileTemp);
                    availableValuesX.Clear();
                    availableValuesY.Clear();
                }
            }

            return tileTemp;
        }

        public Tile[,] generate(MapData loadedData, int size)
        {
            Tile[,] tileTemp = new Tile[size, size];
            Char[] tempChars = loadedData.tileData.ToCharArray();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    string tileKey = null;
                    Char c = tempChars[i * size + j];
                    if (c == '0')
                    {
                        tileKey = waterString;
                    }
                    else if (c == '1')
                    {
                        tileKey = grassString;
                    }
                    else
                    {
                        tileKey = mountainString;
                    }
                    tileTemp[i, j] = new Tile(MapManager.getTileType(tileKey), new Point(i, j));
                }
            }

            List<int> availableValuesX = new List<int>();
            List<int> availableValuesY = new List<int>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    prepareAvailableValues(size, availableValuesX, availableValuesY, i, j);
                    addNeighbours(availableValuesX, availableValuesY, i, j, tileTemp);
                    availableValuesX.Clear();
                    availableValuesY.Clear();
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
                for (int j = 0; j < temp.GetLength(0); j++)
                {
                    temp[i, j] = (temp[i, j] - min) / (max - min);
                }
            }

            return temp;
        }

        private static void prepareAvailableValues(int size, List<int> availableValuesX, List<int> availableValuesY, int i, int y)
        {
            availableValuesX.Add(0);
            availableValuesY.Add(0);

            if (i != 0)
                availableValuesX.Add(-1);
            if (i != size - 1)
                availableValuesX.Add(1);

            if (y != 0)
                availableValuesY.Add(-1);
            if (y != size - 1)
                availableValuesY.Add(1);
        }

        private void addNeighbours(List<int> availableValuesX, List<int> availableValuesY, int i, int j, Tile[,] tileTemp)
        {
            List<Tile> tmpTiles = new List<Tile>();
            foreach (int x in availableValuesX)
            {
                foreach(int y in availableValuesY)
                {
                    if ((x != 0) || (y != 0))
                    {
                        tmpTiles.Add(tileTemp[i + x, y + j]);
                    }
                }
            }
            tileTemp[i, j].setNeigbhours(tmpTiles);
        }


    }
}