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
    public class ExterminationModule : IModule
    {
        public PlayerData player;
        private List<Unit> warriorUnits = new List<Unit>();
        private int desirableSoldierCount;
        private double attackerToGuardRatio;
        private List<Unit> invadingUnits = new List<Unit>();
        private List<Unit> guardingUnits = new List<Unit>();
        private bool invasionStarted = false;
        private City invasionDestination = null;

        public ExterminationModule(PlayerData player)
        {
            this.player = player;
        }

        public void ProcessData(Unit[] units, City[] cities)
        {
            warriorUnits.Clear();
            List<City> citiesList = cities.ToList();
            desirableSoldierCount = player.settingsAI.Aggresiveness + citiesList.Count;
            attackerToGuardRatio = (8 - player.settingsAI.Aggresiveness) / 8;

            ClearInvadingAndGuardinUnits(units);

            foreach (Unit unit in units)
            {
                if (unit.type.name == soldierString)
                {
                    warriorUnits.Add(unit);
                }
            }

            float neededWarriorsAddition = desirableSoldierCount - warriorUnits.Count;

            if (neededWarriorsAddition > 0)
            {
                for (int i = 0; i < neededWarriorsAddition; i++)
                {
                    if (i >= citiesList.Count())
                    {
                        break;
                    }
                    else
                    {
                        if (citiesList[i].currentCommand == null)
                        {
                            citiesList[i].currentCommand = new BuildUnitCommand(citiesList[i], soldierString);
                        }
                    }
                }

            }

            foreach (Unit unit in warriorUnits)
            {
                if (invadingUnits.Contains(unit) == false && unit.currentCommand == null)
                {
                    unit.currentCommand = GetUnitCommandBasedOnAi(unit);                    
                }
            }

            if (invasionStarted == false)
            {
                PrepareOffensive(warriorUnits);
            }
            else
            {
                if (CityManager.Instance.citytDictionary.ContainsValue(invasionDestination) == false)
                {
                    foreach(Unit unit in invadingUnits)
                    {
                        unit.currentCommand = null;
                    }
                    invasionDestination = null;
                }
            }
        }

        public void RequestSeaTransport(Unit unit, Tile destiny)
        {
            throw new NotImplementedException();
        }

        private ICommand GetUnitCommandBasedOnAi(Unit unit)
        {
            if (player.settingsAI.GetGuardChance() == true)
            {
                guardingUnits.Add(unit);
                return new GuardCommand(unit, player.GetRandomCity());
            }
            else
            {
                return null;
            }
        }

        private void PrepareOffensive(List<Unit> warriorUnits)
        {
            int desirableInvadingUnits = (8 - player.settingsAI.Risk) / 2;

            if (invadingUnits.Count < desirableInvadingUnits)
            {
                foreach (Unit unit in warriorUnits)
                {
                    if (invadingUnits.Contains(unit) == false && unit.currentCommand == null)
                    {
                        invadingUnits.Add(unit);
                    }

                }
            }
            else
            {
                if (invadingUnits.Count == desirableInvadingUnits)
                {
                    invasionDestination = null;
                    foreach (City city in CityManager.Instance.citytDictionary.Values)
                    {
                        if (city.playerId != player.id && player.discoveredTiles.Contains(city.currentTile) == true)
                        {
                            if (invasionDestination == null)
                            {
                                invasionDestination = city;
                            }
                            else
                            {
                                if (invasionDestination.currentTile.position.GetDistance(player.GetCenterOfArea()) > city.currentTile.position.GetDistance(player.GetCenterOfArea()))
                                {
                                    invasionDestination = city;
                                }
                            }
                        }
                    }

                    if (invasionDestination != null)
                    {
                        foreach (Unit unit in invadingUnits)
                        {
                            unit.currentCommand = new AttackCommand(unit, invasionDestination);
                        }
                        invasionStarted = true;
                    }
                }
            }
        }

        private void ClearInvadingAndGuardinUnits(Unit [] units)
        {
            for (int i = guardingUnits.Count - 1; i >= 0 ; i--)
            {
                if (units.Contains(guardingUnits[i]) == false)
                {
                    guardingUnits.Remove(guardingUnits[i]);
                }
            }

            for (int i = invadingUnits.Count - 1; i >= 0; i--)
            {
                if (units.Contains(invadingUnits[i]) == false)
                {
                    invadingUnits.Remove(invadingUnits[i]);
                }
            }

        }

    }
}
