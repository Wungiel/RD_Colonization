namespace RD_Colonization.Code.Data
{
    public class TileData
    {
        public string name;
        public int movementCost;
        public bool land;
        public bool walkable;

        public TileData(string name, int movementCost, bool land, bool walkable)
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