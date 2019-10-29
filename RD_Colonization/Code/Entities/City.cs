﻿using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Entities
{
    public class City
    {
        public Tile position;
        public int playerId = -1;
        public ICommand currentCommand = null;

        public City ()
        {

        }

        public City(int playerId, Tile position)
        {
            this.position = position;
            this.playerId = playerId;
        }

        public void GenerateCash()
        {
            int cash = 0;
            foreach(Tile n in position.neighbours)
            {
                if (n.type.name == grassString)
                    cash++;
            }
            PlayerManager.Instance.GetPlayerById(playerId).ModifyCash(cash);
        }

        public void removeCommand()
        {
            currentCommand = null;
        }
    }
}