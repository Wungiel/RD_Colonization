using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class MapManager : BaseManager<MapManager>
    {
        public Dictionary<Rectangle, Tile> mapDictionary = null;
        private readonly Dictionary<String, TileData> typesDictionary = new Dictionary<String, TileData>();
        private static Random randomGenerator = new Random();

        public MapManager()
        {
            typesDictionary.Add(grassString, new TileData(grassString, 1, true, true));
            typesDictionary.Add(waterString, new TileData(waterString, 1, false, true));
            typesDictionary.Add(mountainString, new TileData(mountainString, 1, true, false));
        }

        public void GenerateMap(int size)
        {
            mapDictionary = new Dictionary<Rectangle, Tile>();
            Tile[,] mapData = new MapGenerator().Generate(size);
            CreateDictionary(mapData);
        }

        public void GenerateMap (string mapName, GraphicsDevice device)
        {
            Texture2D mapTexture = ReadMapFile(mapName, device);
            if (mapTexture == null)
            {
                GenerateMap(40);
            }
            else
            {
                mapDictionary = new Dictionary<Rectangle, Tile>();
                Tile[,] mapData = new MapGenerator().Generate(mapTexture);
                CreateDictionary(mapData);
                CheckCorrectness();
            }
        }

        public Tile GetRandomGrassTile()
        {
            var grassTiles = MapManager.Instance.mapDictionary
                .Where(kv => kv.Value.type.name == grassString).Select(kv => kv.Value).ToList();

            return grassTiles[randomGenerator.Next(grassTiles.Count - 1)];

        }

        public TileData GetTileType(String key)
        {
            TileData temp = null;
            typesDictionary.TryGetValue(key, out temp);
            return temp;
        }

        public HashSet<Tile> GetNeighbours(Tile center, int width = 1)
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
                if (discoveredTile.discoveredByPlayerIds.Contains(playerId) == false)
                {
                    ScoreManager.Instance.AddDiscoveredTilePoint(playerId);
                    discoveredTile.discoveredByPlayerIds.Add(playerId);
                }                
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

        private Texture2D ReadMapFile(string mapName, GraphicsDevice device)
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + slash + mapDataFolderString + slash + mapName + pngExtension;
            if (File.Exists(filePath) == true)
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                Texture2D mapTexture = Texture2D.FromStream(device, fileStream);
                fileStream.Dispose();
                return mapTexture;

            }
            else
            {
                return null;
            }
        }

        private void CheckCorrectness()
        {
            List<Tile> tiles = mapDictionary.Values.ToList();
            int numberOfGrassTiles = 0;
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].type.land == true && tiles[i].type.walkable == true)
                {
                    numberOfGrassTiles++;
                }
                if (numberOfGrassTiles >= 4)
                {
                    return;
                }
            }

            mapDictionary.Clear();
            GenerateMap(40);
        }

    }
}