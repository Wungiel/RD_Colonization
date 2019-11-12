using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.DDA
{
    public class DDAResourceManager : BaseManager<DDAResourceManager>
    {
        public bool canPlayerBeOptimized = false;
        public bool canOptimizeOnlyInvisiblePlayer = true;
        public Dictionary<int, int> playersCooldowns = new Dictionary<int, int>();
        public int cooldown = 5;

        public void StartResourceManager()
        {
            if (TestManager.Instance.usedTest == null || TestManager.Instance.usedTest.useResourceFitting == false)
            {
                return;
            }

            if (TestManager.Instance.usedTest.resourceFittingFrequency != 0)
            {
                cooldown = TestManager.Instance.usedTest.resourceFittingFrequency;
            }

            List<PlayerData> players = PlayerManager.Instance.players;

            foreach (PlayerData player in players)
            {
                playersCooldowns.Add(player.id, cooldown);
            }

            TurnManager.Instance.turnEvent += ManageResources;

        }

        public void ManageResources()
        {
            List<PlayerData> players = PlayerManager.Instance.players;
            List<ResourceManagerPlayer> playersToAnalyze = new List<ResourceManagerPlayer>();
            List<ResourceManagerPlayer> playersToOptimize = new List<ResourceManagerPlayer>();
            float playerScore = ScoreManager.Instance.GetScore(0);

            foreach (PlayerData player in players)
            {
                if (player.id == 0)
                {
                    playersToAnalyze.Add(new ResourceManagerPlayer(player.id, ScoreManager.Instance.GetScore(player.id), playerScore, true));
                }
                else
                {
                    playersToAnalyze.Add(new ResourceManagerPlayer(player.id, ScoreManager.Instance.GetScore(player.id), playerScore));
                }
            }

            playersToAnalyze = playersToAnalyze.OrderBy(o => o.score).ToList();
            float maxScore = playersToAnalyze.Last().score;

            foreach (ResourceManagerPlayer player in playersToAnalyze)
            {
                player.NormalizeScore(maxScore);
                player.GetScoreDifferenceToPlayer();
                if (player.CanBeOptimized(maxScore) == true && playersCooldowns[player.id] >= cooldown)
                {
                    playersToOptimize.Add(player);
                }
            }

            foreach (ResourceManagerPlayer player in playersToOptimize)
            {
                player.Optimize(maxScore);
                playersCooldowns[player.id] = 0;
            }

            foreach (PlayerData player in players)
            {
                playersCooldowns[player.id]++;
            }
        }



        private class ResourceManagerPlayer
        {
            public int id;
            public float score;
            public float normalizedScore;
            public float playerScore;
            public float differenceToPlayer;
            public bool isPlayer = false;
            public bool isBetterThanPlayer = false;

            public ResourceManagerPlayer(int id, float score, float playerScore, bool isPlayer = false)
            {
                this.id = id;
                this.score = score;
                this.playerScore = playerScore;
                this.isPlayer = isPlayer;

                if (score > playerScore)
                {
                    isBetterThanPlayer = true;
                }
            }

            public void NormalizeScore(float maxScore)
            {
                normalizedScore = score / maxScore;
            }

            public void GetScoreDifferenceToPlayer()
            {
                differenceToPlayer = playerScore - score;
            }

            public bool CanBeOptimized(float maxScore)
            {
                float normalizedPlayerScore = playerScore / maxScore;

                if (PlayerManager.Instance.GetPlayerById(id).isDefeated == true)
                {
                    return false;
                }

                if (isPlayer == true && DDAResourceManager.Instance.canPlayerBeOptimized == false)
                {
                    return false;
                }
                else if (isPlayer == true)
                {
                    return true;
                }
                else if (Math.Abs(normalizedPlayerScore - normalizedScore) > 0.05 && (Math.Abs(score - playerScore)) > 30)
                {
                    return true;
                }

                return false;
            }

            public void Optimize(float maxScore)
            {
                float normalizedPlayerScore = playerScore / maxScore;

                if (isBetterThanPlayer == true && TestManager.Instance.usedTest.canAffectPlayer > 0)
                {
                    //Zoptymalizuj gracza
                    OptimizeUser(normalizedPlayerScore);
                    EventSaverManager.Instance.SaveDDAOptimizationEvent(0);
                }
                else if (PlayerManager.Instance.GetPlayerById(id).isDiscoveredByPlayer == true && DDAResourceManager.Instance.canOptimizeOnlyInvisiblePlayer == true)
                {
                    //Przeciwnik komputerowy wykryty, ale nie trzeba zachowywać sekretności
                    OptimizeDiscoveredPlayerSecretly(normalizedPlayerScore);
                    EventSaverManager.Instance.SaveDDAOptimizationEvent(id);
                }
                else
                {
                    //Przeciwnik komputerowy niewykryty albo wykrycie nie ma znaczenia
                    OptimizePlayerDefault(normalizedPlayerScore);
                    EventSaverManager.Instance.SaveDDAOptimizationEvent(id);
                }

                
            }
            
            private void OptimizePlayerDefault(float normalizedPlayerScore)
            {
                PlayerData player = PlayerManager.Instance.GetPlayerById(id);

                if (isBetterThanPlayer == true)
                {
                    player.cashDDABonus -= 4;
                    CityManager.Instance.DestroyCity(player.GetRandomCity());
                }
                else
                {

                    if (player.cash < 0)
                    {
                        CancelDebt(id);
                        AddCivilian(id);
                    }
                    else if (player.GetLastTurnIncome() < player.aiModules.expansion.desirableIncome)
                    {
                        AddCivilian(id);
                    }

                    if (player.aiModules.exploration.discoveredRatio < 0.25)
                    {
                        AddScout(id);
                    }

                    if (normalizedPlayerScore < 0.2)
                    {
                        AddScout(id);
                        AddCivilian(id);
                        AddSoldier(id);

                    }

                    AddCash(id);
                }
                
            }

            private void OptimizeUser(float normalizedPlayerScore)
            {
                if (TestManager.Instance.usedTest.canAffectPlayer == 1)
                {
                    PlayerManager.Instance.GetPlayerById(0).attackDDABonus += 0.5f;
                }
                else if (TestManager.Instance.usedTest.canAffectPlayer == 2)
                {
                    PlayerManager.Instance.GetPlayerById(0).cashDDABonus += 4;
                }
                else
                {
                    if (PlayerManager.Instance.GetPlayerById(0).cash < 0)
                    {
                        CancelDebt(0);
                    }
                    AddCivilian(0);
                }
            }

            private void OptimizeDiscoveredPlayerSecretly(float normalizedPlayerScore)
            {
                if (Math.Abs(normalizedPlayerScore - normalizedScore) > 0.3)
                {
                    if (isBetterThanPlayer == true)
                    {
                        PlayerManager.Instance.GetPlayerById(id).cashDDABonus -= 4;
                    }
                    else
                    {
                        PlayerManager.Instance.GetPlayerById(id).cashDDABonus += 4;
                    }

                }
                else
                {
                    if (isBetterThanPlayer == true)
                    {
                        PlayerManager.Instance.GetPlayerById(id).attackDDABonus -= 0.2f;
                    }
                    else
                    {
                        PlayerManager.Instance.GetPlayerById(id).attackDDABonus += 0.2f;
                    }
                }
            }

            private void AddCivilian(int changedPlayerId)
            {
                Tile safeTile = MapManager.Instance.GetSafeDiscoveredTile(changedPlayerId);
                if (safeTile != null)
                    UnitManager.Instance.AddNewUnit(PlayerManager.Instance.GetPlayerById(changedPlayerId), safeTile, civilianString);
            }

            private void AddSoldier(int changedPlayerId)
            {
                Tile safeTile = MapManager.Instance.GetSafeDiscoveredTile(changedPlayerId);
                if (safeTile != null)
                    UnitManager.Instance.AddNewUnit(PlayerManager.Instance.GetPlayerById(changedPlayerId), safeTile, soldierString);
            }


            private void AddScout(int changedPlayerId)
            {
                Tile safeTile = MapManager.Instance.GetSafeDiscoveredTile(changedPlayerId);
                if (safeTile != null)
                    UnitManager.Instance.AddNewUnit(PlayerManager.Instance.GetPlayerById(changedPlayerId), safeTile, scoutString);
            }

            private void CancelDebt(int changedPlayerId)
            {
                PlayerData player = PlayerManager.Instance.GetPlayerById(changedPlayerId);
                player.cash = 50;
            }

            private void AddCash(int changedPlayerId)
            {
                PlayerData player = PlayerManager.Instance.GetPlayerById(changedPlayerId);
                player.cash += 80;
            }


        }
    }
}
