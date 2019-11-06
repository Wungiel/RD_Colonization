using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.DDA
{
    class AiSettingsChromosome : ChromosomeBase
    {
        public AiSettingsChromosome(int AiSettingsLenght) : base(AiSettingsLenght)
        {            
            for (int i = 0; i < AiSettingsLenght; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        public override IChromosome CreateNew()
        {
            return new AiSettingsChromosome(3);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            Random random = new Random();
            List<int> aiSettings  = new List<int>();
            int data;
            if (DDAEvolutionaryAIManager.Instance.settingsData.Count != 0)
            {
                data = DDAEvolutionaryAIManager.Instance.settingsData.First();
                List<int> digits = data.GetDigits();
                aiSettings.Add(digits[0]);
                aiSettings.Add(digits[1]);
                aiSettings.Add(digits[2]);
                if (geneIndex == 2)
                {
                    DDAEvolutionaryAIManager.Instance.settingsData.RemoveAt(0);
                }

                return new Gene(aiSettings[geneIndex]);
            }
            else
            {
                return new Gene(random.Next(0,8));
            }
        }

        public int ToInt()
        {
            int result = 0;
            for (int i = 0; i < Length; i++)
            {
                result += int.Parse(GetGene(i).ToString()) * (10 * i);
            }

            return result;

        }
    }
}
