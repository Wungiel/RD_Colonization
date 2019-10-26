using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class MapManager : BaseManager<MapManager>
    {
        public Dictionary<Rectangle, Tile> mapDictionary = null;
        public int mapSize = 0;
        private readonly Dictionary<String, TileData> typesDictionary = new Dictionary<String, TileData>();

        public MapManager()
        {
            typesDictionary.Add(grassString, new TileData(grassString, 1, true, true));
            typesDictionary.Add(waterString, new TileData(waterString, 1, false, true));
            typesDictionary.Add(mountainString, new TileData(mountainString, 1, true, false));
        }

        public void GenerateMap(int size)
        {
            mapSize = size;
            mapDictionary = new Dictionary<Rectangle, Tile>();
            Tile[,] mapData = new MapGenerator().Generate(size);
            CreateDictionary(mapData);
        }

        public bool LoadMap(MapData loadedData)
        {
            if (loadedData != null)
            {
                mapSize = loadedData.size;
                mapDictionary = new Dictionary<Rectangle, Tile>();
                Tile[,] mapData = new MapGenerator().Generate(loadedData, mapSize);
                CreateDictionary(mapData);
                return true;
            }
            else
                return false;
        }

        public TileData GetTileType(String key)
        {
            TileData temp = null;
            typesDictionary.TryGetValue(key, out temp);
            return temp;
        }

        private void CreateDictionary(Tile[,] mapData)
        {
            foreach (Tile t in mapData)
            {
                Rectangle keyRectangle = new Rectangle(t.position.X * 64, t.position.Y * 64, 64, 64);
                mapDictionary.Add(keyRectangle, t);
            }
        }
    }
}