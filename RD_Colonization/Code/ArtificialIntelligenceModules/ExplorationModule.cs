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
    public class ExplorationModule : IModule
    {
        public PlayerData player;
        private List<Unit> scoutUnits = new List<Unit>();

        public ExplorationModule(PlayerData player)
        {
            this.player = player;
        }

        public void ProcessData(Unit[] units, City[] cities)
        {
            scoutUnits.Clear();

            foreach (Unit unit in units)
            {
                if (unit.type.name == scoutString)
                {
                    scoutUnits.Add(unit);
                }
            }
        }

        public void RequestSeaTransport(Unit unit, Tile destiny)
        {
            throw new NotImplementedException();
        }
    }
}
