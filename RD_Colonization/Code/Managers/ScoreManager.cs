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
        public Dictionary<int, ScoreData> playersScores = new Dictionary<int, ScoreData>();


        public void SetUpScoreManager()
        {
            foreach (PlayerData player in PlayerManager.Instance.players)
            {
                playersScores.Add(player.id, new ScoreData());
            }
        }

        public void SetUpNewScoreDataFromTest(TestData testData)
        {
            scoreForNewCity = testData.buildCityScore;
            scoreForNewUnit = testData.buildUnitScore;
            scoreForDiscoveredTile = testData.discoverTileScore;
            scoreForDestroyedCity = testData.destroyEnemyCityScore;
            scoreForDestroyedUnit = testData.destroyEnemyUnitScore;
        }

        public float CalculateScore(ScoreData scoreData)
        {
            float score = 0;
            score += scoreData.builtCities * scoreForNewCity;
            score += scoreData.builtUnits * scoreForNewUnit;
            score += scoreData.discoveredTiles * scoreForDiscoveredTile;
            score += scoreData.destroyedEnemyCities * scoreForDestroyedCity;
            score += scoreData.destroyedEnemyUnits * scoreForDestroyedUnit;
            return score;
        }

        public float GetScore(int id)
        {
            return CalculateScore(playersScores[id]);
        }

        public void AddNewCityPoint(int id)
        {
            playersScores[id].builtCities++;
        }

        public void AddNewUnitPoint(int id)
        {
            playersScores[id].builtUnits++;
        }

        public void AddDiscoveredTilePoint(int id)
        {
            playersScores[id].discoveredTiles++;
        }

        public void AddDestroyedCityPoint(int id)
        {
            playersScores[id].destroyedEnemyCities++;
        }

        public void AddDestroyedUnitPoint(int id)
        {
            playersScores[id].destroyedEnemyUnits++;
        }
        
    }
}
