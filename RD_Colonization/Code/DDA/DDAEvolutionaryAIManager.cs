using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;


namespace RD_Colonization.Code.DDA
{
    public class DDAEvolutionaryAIManager : BaseManager<DDAEvolutionaryAIManager>
    {
        public List<AiEvolutionaryAlgorithmData> settingsScoreData = new List<AiEvolutionaryAlgorithmData>();
        public List<int> settingsData = new List<int>();

        public int frequency = 10;
        public int turnsSinceLastOptimization = 0;

        public void StartEvolutionManager()
        {
            if (TestManager.Instance.usedTest == null || TestManager.Instance.usedTest.useEvolution == false)
            {
                if (HistoryManagr.Instance.savingHistory == false)
                {
                    return;
                }
            }

            if (TestManager.Instance.usedTest == null)
            {
                frequency = 1;
            }
            else
            {
                if (TestManager.Instance.usedTest.evolutionFrequency == 0)
                {
                    frequency = 10;
                }
                else
                {
                    frequency = TestManager.Instance.usedTest.evolutionFrequency;
                }
            }
            TurnManager.Instance.turnEvent += OptimizeAI;
        }


        public void OptimizeAI()
        {
            
            if (frequency != turnsSinceLastOptimization)
            {
                turnsSinceLastOptimization++;
                return;
            }

            GenerateScoreData();

            int maxSize = GetMaxSize();
            AiSettingsChromosome intelligenceChromosome = new AiSettingsChromosome(3);
            Population population = new Population(maxSize, maxSize, intelligenceChromosome);

            FuncFitness fitness;
            if (HistoryManagr.Instance.savingHistory == true)
            {
                fitness = new FuncFitness((c) => { return GetHistoryFitness(c); });
            }
            else
            {
                fitness = new FuncFitness((c) => { return GetFitness(c); });
            }

            var selection = new EliteSelection();
            var crossover = new UniformCrossover(0.5f);
            var mutation = new DisplacementMutation();
            var termination = new GenerationNumberTermination(2);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            List<IChromosome> savedChromosomes = new List<IChromosome>(); 
            ga.Termination = termination;
            ga.GenerationRan += (sender, e) =>
            {
                if (ga.GenerationsNumber == 2)
                {
                    savedChromosomes = (List<IChromosome>)ga.Population.CurrentGeneration.Chromosomes;
                }
            };

            ga.Start();

            turnsSinceLastOptimization = 0;
            settingsScoreData.Clear();
            settingsData.Clear();

            SetGeneratedData(savedChromosomes);
            foreach (PlayerData player in PlayerManager.Instance.players)
            {                
                EventSaverManager.Instance.SaveDDAEvolutionAIEvent(player.id);
            }            
        }

        private void GenerateScoreData()
        {
            TestData test = TestManager.Instance.usedTest;
            int startingIndex = 1;

            if (test != null && test.canEvolutionUseAIUserParameter == true)
            {
                startingIndex = 0;;
            }

            for (int i = startingIndex; i < PlayerManager.Instance.players.Count; i++)
            {
                settingsScoreData.Add(new AiEvolutionaryAlgorithmData(PlayerManager.Instance.players[i]));
                settingsData.Add(PlayerManager.Instance.players[i].settingsAI.SaveSettingsIntoInt());
            }

            if (test != null && test.canUseHistory == true)
            {
                for (int i = 0; i < HistoryManagr.Instance.historyPerPlayer.Count; i++)
                {
                    int turnNumber = TurnManager.Instance.TurnNumber;
                    HistoryData history = HistoryManagr.Instance.historyPerPlayer[i];
                    settingsScoreData.Add(new AiEvolutionaryAlgorithmData(int.Parse(history.aiSettings[turnNumber]), history.scores[turnNumber]));
                    settingsData.Add(int.Parse(history.aiSettings[turnNumber]));
                }
            }

        }

        private void SetGeneratedData(List<IChromosome> newValues)
        {
            TestData test = TestManager.Instance.usedTest;
            int startingIndex = 1;

            for (int i = startingIndex; i < PlayerManager.Instance.players.Count; i++)
            {
                PlayerManager.Instance.players[i].settingsAI.SetSettingsFromString(newValues.First().ToString());
                newValues.RemoveAt(0);
            }
        }


        private int GetMaxSize()
        {
            TestData test = TestManager.Instance.usedTest;

            int intelligenceSize = 3;

            if (test != null && test.canEvolutionUseAIUserParameter == true)
            {
                intelligenceSize += 1;
            }

            if (test != null && test.canUseHistory == false)
            {
                return intelligenceSize;
            }
            else
            {
                intelligenceSize = settingsScoreData.Count;
                return intelligenceSize;
            }
        }

        private float GetFitness(IChromosome c) 
        {
            AiSettingsChromosome chromosome = (AiSettingsChromosome)c;

            int aiSettings = chromosome.ToInt();
            AiEvolutionaryAlgorithmData toDelete = null;
            float fitness = 0;

            foreach (AiEvolutionaryAlgorithmData data in settingsScoreData)
            {
                if (data.aiSettings == aiSettings)
                {
                    fitness = data.score;
                    toDelete = data;
                }
            }

            float maxScore = ScoreManager.Instance.GetTheHighestScoreValue();
            float playerScore = ScoreManager.Instance.GetScore(0);

            fitness = fitness / maxScore;
            playerScore = playerScore / maxScore;

            fitness = 1 - Math.Abs(playerScore - fitness);

            if (toDelete != null)
            {
                settingsScoreData.Remove(toDelete);
            }

            return fitness;
        }

        private float GetHistoryFitness(IChromosome c)
        {
            AiSettingsChromosome chromosome = (AiSettingsChromosome)c;

            int aiSettings = chromosome.ToInt();
            float fitness = 0;
            AiEvolutionaryAlgorithmData toDelete = null;

            foreach (AiEvolutionaryAlgorithmData data in settingsScoreData)
            {
                if (data.aiSettings == aiSettings)
                {
                    fitness = data.score;
                    toDelete = data;
                }
            }

            if (toDelete != null)
            {
                settingsScoreData.Remove(toDelete);
            }

            return fitness;
        }


        public class AiEvolutionaryAlgorithmData
        {
            public int aiSettings;
            public float score;

            public AiEvolutionaryAlgorithmData(int settings, float score)
            {
                this.aiSettings = settings;
                this.score = score;
            }

            public AiEvolutionaryAlgorithmData(PlayerData player)
            {
                this.aiSettings = player.settingsAI.SaveSettingsIntoInt();
                this.score = ScoreManager.Instance.GetScore(player.id);
            }

            public AiEvolutionaryAlgorithmData()
            {

            }

        }
    }
}
