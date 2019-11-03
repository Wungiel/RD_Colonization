using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void OptimizeAI()
        {
            IntegerChromosome intelligenceChromosome = new IntegerChromosome(0, 8888);
            Population population = new Population(4, GetMaxSize(), intelligenceChromosome);
            FuncFitness fitness = new FuncFitness((c) => { return GetFitness(c); });

            var selection = new EliteSelection();
            var crossover = new PositionBasedCrossover();

        }

        private int GetMaxSize()
        {
            if (TestManager.Instance.usedTest != null && TestManager.Instance.usedTest.canUseHistory == true)
            {
                return 4;
            }
            else
            {
                return 4;
            }
        }

        private int GetFitness(IChromosome c)
        {
            return 0;
        }
    }
}
