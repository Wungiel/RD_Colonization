using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    public class HistoryData
    {
        public int playerId;
        public List<string> aiSettings = new List<string>();
        public List<float> scores = new List<float>();

        public HistoryData(int playerId)
        {
            this.playerId = playerId;
        }
    }
}
