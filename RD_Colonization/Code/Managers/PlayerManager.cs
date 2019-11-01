using Microsoft.Xna.Framework;
using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class PlayerManager : BaseManager<PlayerManager>
    {
        public List<PlayerData> players = new List<PlayerData>();
        public Color[] playerColors = { Color.Red, Color.Blue, Color.Green, Color.White };
        public PlayerData currentPlayer = null;

        public void SetUpPlayers(bool createLivePlayer = true)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerData tmpPlayer = new PlayerData(playerColors[i]);
                              
                players.Add(tmpPlayer);
            }

            ScoreManager.Instance.SetUpScoreManager();

            int remainingPlayers = 4;
            foreach(PlayerData player in players)
            {
                Tile tile = MapManager.Instance.GetStartingTile(remainingPlayers);
                UnitManager.Instance.AddNewUnit(player, tile, civilianString);
                remainingPlayers--;
            }

            if (createLivePlayer == true)
            {
                players[0].SetLivingPlayerControl();
            }

            
            currentPlayer = players[0];
            UnitManager.Instance.ChangeCurrentUnit(currentPlayer);
            TurnManager.Instance.changePlayerEvent += ExecuteCommands;
        }

        public void SwitchPlayer()
        {
            if (players.Last().id == currentPlayer.id)
            {
                currentPlayer = players[0];
            }
            else
            {
                currentPlayer = players[players.IndexOf(currentPlayer) + 1];
            }
        }

        public int GetCurrentPlayerIndex()
        {
            return players.IndexOf(currentPlayer);
        }

        public PlayerData GetPlayerByUnit(Unit unit)
        {
            return GetPlayerById(unit.playerId);
        }

        public PlayerData GetPlayerByCity(City city)
        {
            return GetPlayerById(city.playerId);
        }

        public PlayerData GetPlayerById(int playerId)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].id == playerId)
                {
                    return players[i];
                }
            }
            return null;
        }

        public void ProcessTurn()
        {
            Unit[] units = UnitManager.Instance.GetPlayersUnits(currentPlayer.id);
            City[] cities = CityManager.Instance.GetPlayersCities(currentPlayer.id);
            
            if (IsPlayerDefeated(units, cities) == false)
            {
                CreateSupportMaps(units, cities);
                CommandResources(units, cities);
            }


            TurnManager.Instance.IncreaseTurn();
        }

        private bool IsPlayerDefeated(Unit[] units, City[] cities)
        {
            if (currentPlayer.isDefeated == true)
            {
                return true;
            } 
            else
            {
                if (units.Count() == 0 && cities.Count() == 0)
                {
                    currentPlayer.isDefeated = true;
                    return true;
                }
            }

            return false;
        }

        private void CreateSupportMaps(Unit[] units, City[] cities)
        {
            MapManager.Instance.CreateRiskMap(currentPlayer.id);
            MapManager.Instance.CreateSafetyMap(currentPlayer.id, units, cities);
            MapManager.Instance.CreateTensionMap(currentPlayer.id);
            MapManager.Instance.CreateExplorationMap(currentPlayer.id);
        }

        private void CommandResources(Unit[] units, City[] cities)
        {
            foreach (Unit unit in units)
            {
                if (unit.currentCommand == null)
                    unit.currentCommand = new BuildCityCommand(unit);
            }

            if  (cities.Count() != 0)
            {
                if (units.Count() == 0)
                if (cities[0].currentCommand == null)
                        cities[0].currentCommand = new BuildUnitCommand(cities[0], civilianString);
            }
        }

        private void ExecuteCommands()
        {
            Unit[] units = UnitManager.Instance.GetPlayersUnits(currentPlayer.id);
            City[] cities = CityManager.Instance.GetPlayersCities(currentPlayer.id);

            foreach (Unit u in units)
            {
                    if (u.currentCommand != null && u.currentCommand.Execute() == true)
                    {
                        u.removeCommand();
                    }    
            }

            foreach (City c in cities)
            {
                if (c.currentCommand != null && c.currentCommand.Execute() == true)
                {
                    c.RemoveCommand();
                }
            }
        }
    }
}
