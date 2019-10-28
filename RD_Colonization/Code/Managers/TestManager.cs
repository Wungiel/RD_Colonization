﻿using System;
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

            List<PlayerData> players = PlayerManager.Instance.players;

            foreach(PlayerData player in players)
            {
                playersScore.Add(new SinglePlayerTurnData(player.id));
            }

            TurnManager.Instance.turnEvent += GetData;
        }

        public void GetData()
        {
            List<PlayerData> players = PlayerManager.Instance.players;

            foreach (PlayerData player in players)
            {
                SinglePlayerTurnData score = GetPlayerTestData(player.id);
                score.cashValue.Add(player.cash);
            }
        }

        public void WriteTestResultData()
        {
            if (usedTest == null)
            {
                return;
            }
            string testResultDirectoryPath = System.IO.Directory.GetCurrentDirectory() + slash + resultDataFolderString;
            string fileName = testResultDirectoryPath + slash + "result" + txtExtension;
            File.WriteAllText(fileName, "Results:");
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

    }
}
