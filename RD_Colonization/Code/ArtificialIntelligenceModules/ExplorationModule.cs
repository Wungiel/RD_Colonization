using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.ArtificialIntelligenceModules
{
    public class ExplorationModule : IModule
    {
        public PlayerData player;
        private List<Unit> scoutUnits = new List<Unit>();
        private int desirableScoutsCount;
        private double discoveredRatio;

        public ExplorationModule(PlayerData player)
        {
            this.player = player;
        }

        public void ProcessData(Unit[] units, City[] cities)
        {
            scoutUnits.Clear();
            List<City> citiesList = cities.ToList();
            discoveredRatio = 1.0 - ((double)(player.discoveredTiles.Count()) / (double)MapManager.Instance.mapDictionary.Count);
            desirableScoutsCount = (player.settingsAI.Expansiveness + player.settingsAI.Aggresiveness) / 2;
            desirableScoutsCount = (int) (desirableScoutsCount * discoveredRatio);

            foreach (Unit unit in units)
            {
                if (unit.type.name == scoutString)
                {
                    scoutUnits.Add(unit);
                }
            }

            float neededScoutAddition = desirableScoutsCount - scoutUnits.Count;

            if (neededScoutAddition > 0)
            {
                for (int i = 0; i < neededScoutAddition; i++)
                {
                    if (i >= citiesList.Count())
                    {
                        break;
                    }
                    else
                    {
                        if (citiesList[i].currentCommand == null)
                        {
                            citiesList[i].currentCommand = new BuildUnitCommand(citiesList[i], scoutString);
                        }
                    }
                }

            }

            foreach (Unit unit in scoutUnits)
            {
                if (unit.currentCommand == null)
                {
                    unit.currentCommand = new ExploreCommand(unit);
                }
            }

        }

        public void RequestSeaTransport(Unit unit, Tile destiny)
        {
            throw new NotImplementedException();
        }
    }
}
