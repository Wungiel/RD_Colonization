using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class MapGenerator
    {
        public Tile[,] Generate(int size)
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

            temp = NormalizeArray(temp);

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
                    tileTemp[i, y] = new Tile(MapManager.Instance.GetTileType(tileKey), new Point(i, y));
                }
            }

            List<int> availableValuesX = new List<int>();
            List<int> availableValuesY = new List<int>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    PrepareAvailableValues(size, size, availableValuesX, availableValuesY, i, j);
                    AddNeighbours(availableValuesX, availableValuesY, i, j, tileTemp);
                    availableValuesX.Clear();
                    availableValuesY.Clear();
                }
            }

            return tileTemp;
        }

        internal Tile[,] Generate(Texture2D mapTexture)
        {
            Color[] colors = new Color[mapTexture.Width * mapTexture.Height];
            Tile[,] tileTemp = new Tile[mapTexture.Width, mapTexture.Height];

            mapTexture.GetData<Color>(colors);

            for (int i = 0; i < mapTexture.Width; i++)
            {
                for (int j = 0; j < mapTexture.Height; j++)
                {
                    tileTemp[i, j] = GetTile(colors[i +  j * mapTexture.Width], i, j);
                }
            }

            List<int> availableValuesX = new List<int>();
            List<int> availableValuesY = new List<int>();
            for (int i = 0; i < mapTexture.Width; i++)
            {
                for (int j = 0; j < mapTexture.Height; j++)
                {
                    PrepareAvailableValues(mapTexture.Width, mapTexture.Height, availableValuesX, availableValuesY, i, j);
                    AddNeighbours(availableValuesX, availableValuesY, i, j, tileTemp);
                    availableValuesX.Clear();
                    availableValuesY.Clear();
                }
            }

            return tileTemp;
        }

        private double[,] NormalizeArray(double[,] temp)
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

        private static void PrepareAvailableValues(int width, int height, List<int> availableValuesX, List<int> availableValuesY, int i, int y)
        {
            availableValuesX.Add(0);
            availableValuesY.Add(0);

            if (i != 0)
                availableValuesX.Add(-1);
            if (i != width - 1)
                availableValuesX.Add(1);

            if (y != 0)
                availableValuesY.Add(-1);
            if (y != height - 1)
                availableValuesY.Add(1);
        }

        private void AddNeighbours(List<int> availableValuesX, List<int> availableValuesY, int i, int j, Tile[,] tileTemp)
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
            tileTemp[i, j].SetNeigbhours(tmpTiles);
        }

        private Tile GetTile(Color color, int i, int j)
        {
            String tileKey = string.Empty;
            if (color.R == 255)
            {
                tileKey = grassString;
            } 
            else if (color.G == 255)
            {
                tileKey = grassString;
            }
            else if (color.B == 255)
            {
                tileKey = waterString;
            }
            else
            {
                tileKey = mountainString;
            }

            return new Tile(MapManager.Instance.GetTileType(tileKey), new Point(i, j));
        }


    }
}