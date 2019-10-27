using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    public class PlayerManager : BaseManager<PlayerManager>
    {
        public List<PlayerData> players = new List<PlayerData>();
        public Color[] playerColors = { Color.Red, Color.Blue, Color.Green, Color.White };
        public PlayerData currentPlayer = null;

        public void SetUpPlayers()
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerData tmpPlayer = new PlayerData(playerColors[i]);
                UnitManager.Instance.AddNewBuildingUnit(tmpPlayer);
                players.Add(tmpPlayer);
            }

            currentPlayer = players[0];
            UnitManager.Instance.ChangeCurrentUnit(currentPlayer);
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

    }
}
