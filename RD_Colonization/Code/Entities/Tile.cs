using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    class Tile
    {
        public TileData type;
        public Point position;
        public Tile[] neighbours = new Tile[8];

        public Tile(TileData type, Point position)
        {
            this.type = type;
            this.position = position;
        }

        public void setNeigbhours(Tile[] neighbours)
        {
            this.neighbours = neighbours;
        }
    }
}
