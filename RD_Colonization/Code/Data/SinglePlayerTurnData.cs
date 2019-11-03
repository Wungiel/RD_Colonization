using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    public class SinglePlayerTurnData
    {
        public int playerId = -1;
        public List<float> cashValue = new List<float>();
        public List<string> aiSettings = new List<string>();
        public List<float> scoreValue = new List<float>();
        public List<string> eventList = new List<string>();

        public SinglePlayerTurnData(int playerId)
        {
            this.playerId = playerId;
        }
    }
}
