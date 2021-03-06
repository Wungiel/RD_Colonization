﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD_Colonization.Code.Data;
using static RD_Colonization.Code.StringList;
using RD_Colonization.Code.DDA;
using System.Diagnostics;

namespace RD_Colonization.Code.Managers
{
    public class TestManager : BaseManager<TestManager>
    {
        public List<SinglePlayerTurnData> playersScore = new List<SinglePlayerTurnData>();
        public TestData usedTest = null;
        public Dictionary<int, HistoryData> history = new Dictionary<int, HistoryData>();
        public Stopwatch watch;

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
                EventSaverManager.Instance.StartRegistering();
                DDAResourceManager.Instance.StartResourceManager();
                DDAEvolutionaryAIManager.Instance.StartEvolutionManager();
                ScoreManager.Instance.SetUpNewScoreDataFromTest(usedTest);
                ScreenManager.Instance.SetScreen(gameScreenString);
            }
            else
            {
                StartTimeCount();
                PlayerManager.Instance.SetUpPlayers(false);
                SetUpPlayerScoreRecords();
                EventSaverManager.Instance.StartRegistering();
                HistoryManagr.Instance.ReadHistoryData(usedTest.mapName);
                DDAResourceManager.Instance.StartResourceManager();
                DDAEvolutionaryAIManager.Instance.StartEvolutionManager();
                ScoreManager.Instance.SetUpNewScoreDataFromTest(usedTest);
                PlayerManager.Instance.ProcessTurn();
            }
        }


        public void StartTimeCount()
        {
            watch = System.Diagnostics.Stopwatch.StartNew();
        }

        public void StopTimeCount()
        {
            watch.Stop();
            Debug.WriteLine(watch.ElapsedMilliseconds);
        }


        public void GetData()
        {
            List<PlayerData> players = PlayerManager.Instance.players;

            foreach (PlayerData player in players)
            {
                SinglePlayerTurnData score = GetPlayerTestData(player.id);
                score.cashValue.Add(player.cash);
                score.aiSettings.Add(player.settingsAI.SaveSettingsIntoString());
                score.scoreValue.Add(ScoreManager.Instance.GetScore(player.id));
                score.eventList.Add(EventSaverManager.Instance.GetPlayerEvents(player.id));
            }
        }

        public void WriteTestResultData()
        {
            string testName = string.Empty;

            if (usedTest == null)
            {
                testName = "result";
            }
            else
            {
                testName = usedTest.name;
            }

            string testResultDirectoryPath = System.IO.Directory.GetCurrentDirectory() + slash + resultDataFolderString;
            string fileName = testResultDirectoryPath + slash + testName + underscore + DateTime.Now.Hour + dot + DateTime.Now.Minute + txtExtension;
            File.AppendText(testName);
            for (int i = 0; i < playersScore.Count; i++)
            {
                File.AppendAllText(fileName, JsonManager.Instance.WriteIntoJson<SinglePlayerTurnData>(playersScore[i]) + Environment.NewLine);
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

            TurnManager.Instance.lateTurnEvent += GetData;
        }
                
    }
}
