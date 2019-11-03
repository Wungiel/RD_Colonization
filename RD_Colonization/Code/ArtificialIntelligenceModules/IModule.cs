using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.ArtificialIntelligenceModules
{
    interface IModule
    {
        void ProcessData(Unit[] units, City[] cities);
    }
}
