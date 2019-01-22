using Microsoft.Xna.Framework;

namespace RD_Colonization.Code.Data
{
    public class Tile
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