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
        public List<int> cashValue = new List<int>();
        public List<int> foodValue = new List<int>();
        public List<int> scoreValue = new List<int>();
        public List<string> eventList = new List<string>();

        public SinglePlayerTurnData(int playerId)
        {
            this.playerId = playerId;
        }
    }
}
