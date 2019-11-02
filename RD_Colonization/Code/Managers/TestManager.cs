using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD_Colonization.Code.Data;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class TestManager : BaseManager<TestManager>
    {
        public List<SinglePlayerTurnData> playersScore = new List<SinglePlayerTurnData>();
        public TestData usedTest = null;
        public Dictionary<int, HistoryData> history = new Dictionary<int, HistoryData>();

        public List<String> GetTestFiles()
        {
            string testDirectoryPath = System.IO.Directory.GetCurrentDirectory() + slash + testDataFolderString;

            if (Directory.Exists(testDirectoryPath) == false)
            {
                Directory.CreateDirectory(testDirectoryPath);
            }            

            return Directory.EnumerateFiles(testDirectoryPath, "*", SearchOption.AllDirectories).Select(Path.GetFileName).ToList();
        }

        public TestData GetTestData(string testName)
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + slash + testDataFolderString + slash + testName;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonManager.Instance.ReadJSON<TestData>(json);
            }
            else
            {
                return null;
            }
        }

        public void InitializeTest(TestData test)
        {

            usedTest = test;

            if (usedTest.canPlayerPlay == true)
            {
                PlayerManager.Instance.SetUpPlayers();
                SetUpPlayerScoreRecords();
                ScreenManager.Instance.SetScreen(gameScreenString);
            }
            else
            {
                PlayerManager.Instance.SetUpPlayers(false);
                SetUpPlayerScoreRecords();
                PlayerManager.Instance.ProcessTurn();
            }

            
        }

        public void GetData()
        {
            List<PlayerData> players = PlayerManager.Instance.players;

            foreach (PlayerData player in players)
            {
                SinglePlayerTurnData score = GetPlayerTestData(player.id);
                score.cashValue.Add(player.cash);
                score.scoreValue.Add(ScoreManager.Instance.GetScore(player.id));
            }
        }

        public void WriteTestResultData()
        {
            string testName = string.Empty;

            if (usedTest == null)
            {
                testName = "result";

                string testResultDirectoryPath = System.IO.Directory.GetCurrentDirectory() + slash + resultDataFolderString;
                string fileName = testResultDirectoryPath + slash + testName + underscore + DateTime.Now.Hour + dot + DateTime.Now.Minute + txtExtension;
                File.WriteAllText(fileName, "Results:");
                for (int i = 0; i < playersScore.Count; i++)
                {
                    File.AppendAllText(fileName, JsonManager.Instance.WriteIntoJson<SinglePlayerTurnData>(playersScore[i]) + Environment.NewLine + Environment.NewLine);
                }
            }
        }

        private SinglePlayerTurnData GetPlayerTestData(int id)
        {
            for (int i = 0; i < playersScore.Count; i++)
            {
                if (playersScore[i].playerId == id)
                {
                    return playersScore[i];
                }
            }

            return null;
        }

        private void SetUpPlayerScoreRecords()
        {
            List<PlayerData> players = PlayerManager.Instance.players;

            foreach (PlayerData player in players)
            {
                playersScore.Add(new SinglePlayerTurnData(player.id));
            }

            TurnManager.Instance.turnEvent += GetData;
        }
                
    }
}
