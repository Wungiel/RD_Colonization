using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    [Serializable]
    public class HistoryData
    {
        public Dictionary<int, List<ScoreAISettingsPair>> savedScoreDataPerTurn = new Dictionary<int, List<ScoreAISettingsPair>>();

        [Serializable]
        public  class ScoreAISettingsPair
        {
            string aiSettings;
            float score;
                        
            public ScoreAISettingsPair(string settings, float score)
            {
                this.aiSettings = settings;
                this.score = score;
            }
        }
    }
}
