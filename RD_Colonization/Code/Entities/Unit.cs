using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Entities
{
    public class Unit
    {
        public UnitData type;
        public Tile position;

        public Unit(UnitData type, Tile position)
        {
            this.type = type;
            this.position = position;
        }
    }
}