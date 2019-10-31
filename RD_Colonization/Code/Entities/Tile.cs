using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Data
{
    public class Tile
    {
        public TileData type;
        public Point position;
        public List<Tile> neighbours = new List<Tile>();
        public HashSet<int> discoveredByPlayerIds = new HashSet<int>();
        public Dictionary<int, int> riskValues = new Dictionary<int, int>();
        public Dictionary<int, int> safetyValues = new Dictionary<int, int>();
        public Dictionary<int, int> tensionValues = new Dictionary<int, int>();
        public Dictionary<int, bool> explorationMap = new Dictionary<int, bool>();

        public Tile(TileData type, Point position)
        {
            this.type = type;
            this.position = position;
        }

        public void SetNeigbhours(List<Tile> neighbours)
        {
            this.neighbours = neighbours;
        }

        public Point GetPosition()
        {
            return new Point(position.X * 64, position.Y * 64);
        }

        public Point GetSize()
        {
            return new Point(64, 64);
        }

        public Point2 GetCenter()
        {
            return new Point2(position.X * 64 + 32, position.Y * 64 + 32);
        }

        public int GetValue()
        {
            int value = 0;

            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].type == MapManager.Instance.GetTileType(grassString))
                {
                    value++;
                }
            }

            return value;
        }

        public Tile GetNeighbourTileForNewUnit()
        {
            foreach (Tile t in neighbours)
            {
                if (t.IsFree() == true)
                {
                    return t;
                }
            }

            return null;
        }

        public Tile GetNeighbourTileForNewWaterUnit()
        {
            foreach (Tile t in neighbours)
            {
                if (t.IsFreeForWaterUnit() == true)
                {
                    return t;
                }
            }

            return null;
        }

        public bool BordersUndiscovered(int playerId)
        {
            foreach (Tile t in neighbours)
            {
                if (t.discoveredByPlayerIds.Contains(playerId) == false)
                {
                    return true;
                }
            }

            return false;

        }



        private bool IsFree()
        {
            return type.walkable == true && type.land  == true && UnitManager.Instance.unitDictionary.ContainsKey(this.CreateRectangle()) == false;
        }

        private bool IsFreeForWaterUnit()
        {
            return type.walkable == true && type.land == false && UnitManager.Instance.unitDictionary.ContainsKey(this.CreateRectangle()) == false;
        }

    }
}