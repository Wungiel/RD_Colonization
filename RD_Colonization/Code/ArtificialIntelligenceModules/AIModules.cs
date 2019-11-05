using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.ArtificialIntelligenceModules
{
    public class AIModules
    {
        public ExpansionModule expansion;
        public ExterminationModule extermination;
        public ExplorationModule exploration;
        private PlayerData player;

        public AIModules(PlayerData player)
        {
            this.player = player;
            expansion = new ExpansionModule(player);
            extermination = new ExterminationModule(player);
            exploration = new ExplorationModule(player);
        }

        public void ProcessData(Unit[] units, City[] cities)
        {
            if (player.settingsAI.Expansiveness > player.settingsAI.Aggresiveness)
            {
                exploration.ProcessData(units, cities);
                expansion.ProcessData(units, cities);
                extermination.ProcessData(units, cities);                
            }
            else
            {
                exploration.ProcessData(units, cities);                
                extermination.ProcessData(units, cities);
                expansion.ProcessData(units, cities);
            }

        }
    }
}
