using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RD_Colonization.Code.Data
{
    public class Tile
    {
        public TileData type;
        public Point position;
        public List<Tile> neighbours = new List<Tile>();

        public Tile(TileData type, Point position)
        {
            this.type = type;
            this.position = position;
        }

        public void setNeigbhours(List<Tile> neighbours)
        {
            this.neighbours = neighbours;
        }
    }
}