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

        public TileData GetTileType(String key)
        {
            TileData temp = null;
            typesDictionary.TryGetValue(key, out temp);
            return temp;
        }

        public HashSet<Tile> GetNeighbours(Tile center, int width)
        {
            HashSet<Tile> tiles = new HashSet<Tile>();
            tiles.Add(center);
            while (width > 0)
            {
                HashSet<Tile> newTiles = new HashSet<Tile>();
                foreach (Tile tile in tiles)
                {
                    for (int i = 0; i < tile.neighbours.Count; i++)
                    {
                        newTiles.Add(tile.neighbours[i]);
                    }
                }
                tiles.UnionWith(newTiles);
                width--;
            }

            return tiles;
        }

        public void DiscoverMap(Tile tile, int fieldOfView, int playerId)
        {
            HashSet<Tile> discoveredTiles = GetNeighbours(tile, fieldOfView);
            foreach (Tile discoveredTile in discoveredTiles)
            {
                discoveredTile.discoveredByPlayerIds.Add(playerId);
            }
        }

        public Tile[] GetDiscoveredTiles(int playerId)
        {
            List<Tile> playerDiscoveredTiles = new List<Tile>();
            foreach (Tile tile in mapDictionary.Values)
            {
                if (tile.discoveredByPlayerIds.Contains(playerId))
                {
                    playerDiscoveredTiles.Add(tile);
                }
            }
            return playerDiscoveredTiles.ToArray();
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