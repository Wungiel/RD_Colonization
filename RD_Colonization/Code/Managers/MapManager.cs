using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
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
        public List<Tile> startingTiles = new List<Tile>();
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

        public bool GenerateMap (string mapName, GraphicsDevice device)
        {
            Texture2D mapTexture = ReadMapFile(mapName, device);
            if (mapTexture == null)
            {
                GenerateMap(40);
                return false;
            }
            else
            {
                mapDictionary = new Dictionary<Rectangle, Tile>();
                Tile[,] mapData = new MapGenerator().Generate(mapTexture);
                CreateDictionary(mapData);
                return CheckCorrectness();
            }
        }

        public Tile GetStartingTile(int remainingPlayers)
        {
            Random random = new Random();
            Tile startingTile = null;

            if (startingTiles.Count() > remainingPlayers)
            {
                startingTile = startingTiles[random.Next(startingTiles.Count())];
            }
            else if (startingTiles.Count() == remainingPlayers)
            {
                startingTile = startingTiles.First();
            }
            else
            {
                startingTile = GetRandomGrassTile();                
            }
            startingTiles.Remove(startingTile);
            return startingTile;
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
                    PlayerManager.Instance.GetPlayerById(playerId).AddToDiscoveredHashset(discoveredTile);
                }                
                
            }
        }

        public Tile[] GetDiscoveredTiles(int playerId)
        {
            return PlayerManager.Instance.GetPlayerById(playerId).discoveredTiles.ToArray();
        }

        public Tile GetSafeDiscoveredTile(int playerId)
        {
            Random random = new Random();
            Tile[] discoveredTiles = GetDiscoveredTiles(playerId);
            List<Tile> safeTiles = new List<Tile>();

            foreach(Tile tile in discoveredTiles)
            {
                if (tile.safetyValues.ContainsKey(playerId) == true && tile.tensionValues.ContainsKey(playerId) == false)
                {
                    safeTiles.Add(tile);
                }
            }

            return safeTiles[random.Next(safeTiles.Count - 1)];
        }

        public void CreateRiskMap(int playerId)
        {
            Tile [] discoveredTiles = GetDiscoveredTiles(playerId);

            foreach (Tile t in discoveredTiles)
            {
                if (t.BordersUndiscovered(playerId) == true)
                {
                    t.riskValues[playerId] = 1;
                }
                else
                {
                    t.riskValues[playerId] = 0;
                }
            }

            Unit[] enemyUnits = UnitManager.Instance.CollectEnemyUnitsFromTiles(discoveredTiles, playerId);

            foreach (Unit enemyUnit in enemyUnits)
            {
                foreach (Tile t in GetNeighbours(enemyUnit.currentTile, enemyUnit.type.speed))
                {
                    if (discoveredTiles.Contains(t) == true)
                    {
                        t.riskValues[playerId] += enemyUnit.type.strenght;
                    }
                }
            }

        }

        public void CreateSafetyMap(int playerId, Unit[] units, City[] cities)
        {
            Tile[] discoveredTiles = GetDiscoveredTiles(playerId);
            foreach (Tile t in discoveredTiles)
            {
                t.safetyValues[playerId] = 0;
            }

            foreach(City c in cities)
            {
                foreach (Tile t in GetNeighbours(c.currentTile, 4))
                {
                    if (discoveredTiles.Contains(t) == true)
                    {
                        t.safetyValues[playerId] += 2;
                    }
                }

            }

            foreach (Unit u in units)
            {
                foreach (Tile t in GetNeighbours(u.currentTile, u.type.speed))
                {
                    if (discoveredTiles.Contains(t) == true)
                    {
                        t.safetyValues[playerId] += u.type.strenght;
                    }
                }
            }

            foreach(City c in cities)
            {
                foreach (Tile t in GetNeighbours(c.currentTile, 3))
                {
                    if (discoveredTiles.Contains(t) == true)
                    {
                        t.safetyValues[playerId] += 3;
                    }
                }
            }
        }

        public void CreateTensionMap(int playerId)
        {
            Tile[] discoveredTiles = GetDiscoveredTiles(playerId);
            foreach (Tile t in discoveredTiles)
            {
                t.tensionValues[playerId] = t.safetyValues[playerId] - t.riskValues[playerId];
            }
        }

        public Tile[] CreateExplorationMap(int playerId)
        {
            Tile[] discoveredTiles = GetDiscoveredTiles(playerId);
            List<Tile> tilesForExploration = new List<Tile>();

            foreach (Tile t in discoveredTiles)
            {
                if (t.BordersUndiscovered(playerId) == true && t.type.walkable == true)
                {
                    t.explorationMap[playerId] = true;
                    tilesForExploration.Add(t);
                }
                else
                {
                    t.explorationMap[playerId] = false;
                }
            }
            return tilesForExploration.ToArray();
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

        private bool CheckCorrectness()
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
                    return true;
                }
            }

            mapDictionary.Clear();
            GenerateMap(40);
            return false;
        }

    }
}