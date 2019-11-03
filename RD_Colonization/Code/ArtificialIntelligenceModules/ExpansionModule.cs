using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.ArtificialIntelligenceModules
{
    public class ExpansionModule : IModule
    {
        public PlayerData player;
        private List<Unit> civilianUnits = new List<Unit>();

        public ExpansionModule(PlayerData player)
        {
            this.player = player;
        }

        public void ProcessData(Unit[] units, City[] cities)
        {
            civilianUnits.Clear();

            foreach (Unit unit in units)
            {
                if (unit.type.name == civilianString)
                {
                    civilianUnits.Add(unit);
                }
            }
        }
    }
}
