using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.ArtificialIntelligenceModules
{
    public class ExpansionModule : IModule
    {
        public PlayerData player;
        private List<Unit> civilianUnits = new List<Unit>();
        private int desirableIncome;

        public ExpansionModule(PlayerData player)
        {
            this.player = player;
        }

        public void ProcessData(Unit[] units, City[] cities)
        {
            civilianUnits.Clear();
            List<City> citiesList = cities.ToList();
            desirableIncome = player.settingsAI.Expansiveness * 6;

            foreach (Unit unit in units)
            {
                if (unit.type.name == civilianString)
                {
                    civilianUnits.Add(unit);
                }
            }

            float neededIncomeAddition = desirableIncome - player.GetLastTurnIncome();
            if (neededIncomeAddition > 0)
            {
                int newNecessaryCities = (int) neededIncomeAddition / 8;
                int newNecessaryCivilians = newNecessaryCities - civilianUnits.Count();

                if (newNecessaryCivilians > 0)
                {
                    for (int i = 0; i < newNecessaryCivilians; i++)
                    {
                        if (i >= citiesList.Count())
                        {
                            break;
                        }
                        else
                        {
                            if (citiesList[i].currentCommand == null)
                            {
                                citiesList[i].currentCommand = new BuildUnitCommand(citiesList[i], civilianString);
                            }                            
                        }

                    }
                }
            }

            foreach (Unit unit in civilianUnits)
            {
                if (unit.currentCommand == null)
                {
                    unit.currentCommand = new BuildCityCommand(unit);
                }
            }
        }

        public void RequestSeaTransport(Unit unit, Tile destiny)
        {
            throw new NotImplementedException();
        }
    }
}
