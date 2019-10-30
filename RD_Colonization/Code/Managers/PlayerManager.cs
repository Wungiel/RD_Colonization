﻿using Microsoft.Xna.Framework;
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
                Tile tile = MapManager.Instance.GetRandomGrassTile();
                UnitManager.Instance.AddNewUnit(tmpPlayer, tile, civilianString);
                players.Add(tmpPlayer);
            }

            if (createLivePlayer == true)
            {
                players[0].SetLivingPlayerControl();
            }
            
            currentPlayer = players[0];
            UnitManager.Instance.ChangeCurrentUnit(currentPlayer);
            TurnManager.Instance.turnEvent += ExecuteCommands;
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
            
            CreateSupportMaps(units, cities);
            CommandResources(units, cities);

            TurnManager.Instance.IncreaseTurn();
        }

        private void CreateSupportMaps(Unit[] units, City[] cities)
        {

        }

        private void CommandResources(Unit[] units, City[] cities)
        {

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
