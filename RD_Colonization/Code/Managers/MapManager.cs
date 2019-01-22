using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public static class MapManager
    {
        public static Dictionary<Rectangle, Tile> mapDictionary = null;
        public static int mapSize = 0;
        private static readonly Dictionary<String, TileData> typesDictionary = new Dictionary<String, TileData>();

        static MapManager()
        {
            typesDictionary.Add(grassString, new TileData(grassString, 1, true, true));
            typesDictionary.Add(waterString, new TileData(waterString, 1, true, true));
            typesDictionary.Add(mountainString, new TileData(mountainString, 1, true, false));
        }

        public static void generateMap(int size)
        {
            mapSize = size;
            mapDictionary = new Dictionary<Rectangle, Tile>();
            Tile[,] mapData = new MapGenerator().generate(size);
            createDictionary(mapData);
        }

        public static bool loadMap(MapData loadedData)
        {
            if (loadedData != null)
            {
                mapSize = loadedData.size;
                mapDictionary = new Dictionary<Rectangle, Tile>();
                Tile[,] mapData = new MapGenerator().generate(loadedData, mapSize);
                createDictionary(mapData);
                return true;
            }
            else
                return false;
        }

        public static TileData getTileType(String key)
        {
            TileData temp = null;
            typesDictionary.TryGetValue(key, out temp);
            return temp;
        }

        private static void createDictionary(Tile[,] mapData)
        {
            foreach (Tile t in mapData)
            {
                Rectangle keyRectangle = new Rectangle(t.position.X * 64, t.position.Y * 64, 64, 64);
                mapDictionary.Add(keyRectangle, t);
            }
        }
    }
}