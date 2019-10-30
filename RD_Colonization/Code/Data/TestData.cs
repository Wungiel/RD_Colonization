using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    public class TestData
    {
        public String name;
        public String mapName;
        public bool useFixedStartPoints;

        public bool useEvolution;
        public bool canEvolutionUseAIUserParameter;
        public bool canUseHistory;
        public int evolutionFrequency;

        public bool useResourceFitting;
        public bool canAffectPlayer;
        public bool isMaintainingSecrecy;
        public int resourceFittingFrequency;

        public float buildCityScore;
        public float buildUnitScore;
        public float discoverTileScore;
        public float destroyEnemyCityScore;
        public float destroyEnemyUnitScore;

        public bool canPlayerPlay;
        public int numberOfTurns;
    }
}
