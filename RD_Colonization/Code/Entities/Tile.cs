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
    }
}