using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Entities
{
    public class City
    {
        public Tile position;

        public City(Tile position)
        {
            this.position = position;
        }
    }
}