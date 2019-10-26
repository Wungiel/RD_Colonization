using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using static RD_Colonization.Code.RectangleHelper;

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

        public Rectangle GetPosition()
        {
            return CreateRectangle(position.position);
        }
    }
}