using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Components
{
    class CivilizationData
    {
        public readonly string civilizationName;
        public readonly string civilizationLeader;
        public readonly int artNumber;
        
        public CivilizationData(string civilizationName, string civilizationLeader, int artNumber)
        {
            this.civilizationName = civilizationName;
            this.civilizationLeader = civilizationLeader;
            this.artNumber = artNumber;
        }
    }
}
