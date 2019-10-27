using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Entities
{
    public class Unit
    {
        public int playerId = -1;
        public UnitData type;
        public Tile currentTile;

        public Unit(UnitData type, Tile position, int playerId)
        {
            this.type = type;
            this.currentTile = position;
            this.playerId = playerId;
        }

        public Rectangle GetPosition()
        {
            return currentTile.CreateRectangle();
        }
    }
}