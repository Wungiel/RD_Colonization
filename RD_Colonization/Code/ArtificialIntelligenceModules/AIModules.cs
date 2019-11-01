using RD_Colonization.Code.Data;
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
        public ExploitationModule exploitation;
        public ExplorationModule exploration;

        public AIModules(PlayerData player)
        {
            expansion = new ExpansionModule(player);
            exploitation = new ExploitationModule(player);
            exploration = new ExplorationModule(player);
        }

    }
}
