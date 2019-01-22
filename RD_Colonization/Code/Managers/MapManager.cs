using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    static class MapManager
    {

        public static Dictionary<Rectangle, Tile> mapDictionary = null;
        public static int mapSize = 0;
        private static Dictionary<String, TileType> typesDictionary = new Dictionary<String, TileType>();

        static MapManager()
        {
            typesDictionary.Add(grassString, new TileType(grassString, 1, true, true));
            typesDictionary.Add(waterString, new TileType(waterString, 1, true, true));
            typesDictionary.Add(mountainString, new TileType(mountainString, 1, true, false));
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

        public static TileType getTileType(String key)
        {
            TileType temp = null;
            typesDictionary.TryGetValue(key, out temp);
            return temp;
        }

        private static void createDictionary(Tile[,] mapData)
        {
            foreach(Tile t in mapData)
            {
                Rectangle keyRectangle = new Rectangle(t.position.X * 64, t.position.Y * 64, 64, 64);
                mapDictionary.Add(keyRectangle, t);
            }
        }

    }
}