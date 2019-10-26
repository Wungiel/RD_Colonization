using System.Collections.Generic;

namespace RD_Colonization.Code.Data
{
    public class TileData
    {
        public string name;
        public int movementCost;
        public bool land;
        public bool walkable;
        public HashSet<int> discoveredByPlayerIds = new HashSet<int>();

        public TileData(string name, int movementCost, bool land, bool walkable)
        {
            this.name = name;
            this.movementCost = movementCost;
            this.land = land;
            this.walkable = walkable;
        }
    }
}