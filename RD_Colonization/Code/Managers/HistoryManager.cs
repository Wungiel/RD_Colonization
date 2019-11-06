using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.DDA;
using static RD_Colonization.Code.StringList;


namespace RD_Colonization.Code.Managers
{
    public class HistoryManagr : BaseManager<HistoryManagr>
    {
        public bool savingHistory = false;
        private List<string> mapNames = new List<string>();
        private int currentMapIndex = 0;
        private GraphicsDevice device;
        private List<HistoryData> historyPerPlayer = null;
        private int numberOfPlaysToGenerateHistory = 10;
        private int currentPlayIndex = 0;

        public void GenerateHistory(GraphicsDevice device)
        {
            mapNames = GetMapsForHistory();
            savingHistory = true;
            this.device = device;

            if (mapNames.Count != 0)
            {
                StartHistoryGeneratingPlay(device);
            }
        }
        
        public void ContinueGenerating()
        {
            SaveDataHistoryToFile();

            currentMapIndex++;
            if (mapNames.Count > currentMapIndex)
            {                
                StartHistoryGeneratingPlay(device);
            }
            else
            {
                savingHistory = false;
                currentMapIndex = 0;
                TurnManager.Instance.turnEvent -= SaveDataToHistory;
            }
        }

        private void CreateHistoryForNewMap()
        {
            historyPerPlayer = new List<HistoryData>();

            foreach (PlayerData player in PlayerManager.Instance.players)
            {
                historyPerPlayer.Add(new HistoryData(player.id));
            }

        }

        private List<string> GetMapsForHistory()
        {
            string mapFolderPath = System.IO.Directory.GetCurrentDirectory() + slash + mapDataFolderString;
            List <String> maps = Directory.EnumerateFiles(mapFolderPath, "*", SearchOption.AllDirectories).Select(Path.GetFileName).ToList();
            for (int i = 0; i< maps.Count; i++)
            {
                maps[i] = maps[i].Substring(0, maps[i].Length - 4);
            }

            return maps;
        }

        private void StartHistoryGeneratingPlay(GraphicsDevice device)
        {
            TurnManager.Instance.turnEvent += SaveDataToHistory;
            MapManager.Instance.GenerateMap(mapNames[currentMapIndex], device);
            PlayerManager.Instance.SetUpPlayers(false);
            CreateHistoryForNewMap();
            DDAEvolutionaryAIManager.Instance.StartEvolutionManager();
            PlayerManager.Instance.ProcessTurn();
        }

        private void SaveDataToHistory()
        {
            foreach(HistoryData history in historyPerPlayer)
            {
                PlayerData player = PlayerManager.Instance.GetPlayerById(history.playerId);
                history.aiSettings.Add(player.settingsAI.SaveSettingsIntoString());
                history.scores.Add(ScoreManager.Instance.playersScores[player.id].GetTotalScore());
            }
        }

        private void SaveDataHistoryToFile()
        {
            string mapFolderPath = System.IO.Directory.GetCurrentDirectory() + slash + mapDataFolderString;
            string fileName = mapFolderPath + slash + mapNames[currentMapIndex] + hisExtension;
            File.WriteAllText(fileName, "Results:");
            foreach (HistoryData history in historyPerPlayer)
            {
                File.AppendAllText(fileName, JsonManager.Instance.WriteIntoJson <HistoryData>(history) + Environment.NewLine);
            }
        }




    }
}
