using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    public class ScoreData
    {
        public int builtCities = 0;
        public int builtUnits = 0;
        public int discoveredTiles = 0;
        public int destroyedEnemyCities = 0;
        public int destroyedEnemyUnits = 0;
        private float totalScore;

        public float GetTotalScore()
        {
            totalScore = ScoreManager.Instance.CalculateScore(this);
            return totalScore;
        }
    }
}
