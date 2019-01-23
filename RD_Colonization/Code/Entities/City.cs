using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Entities
{
    public class City
    {
        public Tile position;

        public City(Tile position)
        {
            this.position = position;
        }

        public void generateCash()
        {
            int cash = 0;
            foreach(Tile n in position.neighbours)
            {
                if (n.type.name == grassString)
                    cash++;
            }
            CivilizationManager.addCash(cash);
        }
    }
}