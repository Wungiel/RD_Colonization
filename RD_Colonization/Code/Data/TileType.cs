using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    class TileType
    {
        public string name;
        public int movementCost;
        public bool land;
        public bool walkable;
        
        public TileType(string name, int movementCost, bool land, bool walkable)
        {
            this.name = name;
            if (movementCost < 1)
                movementCost = 1;
            this.movementCost = movementCost;
            this.land = land;
            this.walkable = walkable;
        }
    }
}
