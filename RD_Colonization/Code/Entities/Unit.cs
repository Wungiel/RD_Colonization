using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Entities
{
    public class Unit
    {
        public UnitData type;
        public Tile currentTile;

        public Unit(UnitData type, Tile position)
        {
            this.type = type;
            this.currentTile = position;
        }

        public Rectangle GetPosition()
        {
            return currentTile.CreateRectangle();
        }
    }
}