using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Managers
{
    public class ScoreManager : BaseManager<ScoreManager>
    {

        public float scoreForNewCity = 10;
        public float scoreForNewUnit = 2;
        public float scoreForDiscoveredTile = 0.1f;
        public float scoreForDestroyedCity = 50;
        public float scoreForDestroyedUnit = 5;


        public float CalculateScore(ScoreData scoreData)
        {
            throw new NotImplementedException();
        }
    }
}
