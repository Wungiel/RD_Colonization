using System;
using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Entities
{
    public class City
    {
        public Tile currentTile;
        public int playerId = -1;
        public bool didBuildInThisTurn = false;
        public ICommand currentCommand = null;

        public City ()
        {
            
        }

        public City(int playerId, Tile position)
        {
            this.currentTile = position;
            this.playerId = playerId;
            TurnManager.Instance.turnEvent += AllowToBuildInThisTurn;
        }

        public void GenerateCash()
        {
            int cash = 0;
            foreach(Tile n in currentTile.neighbours)
            {
                if (n.type.name == grassString)
                    cash++;
            }
            PlayerManager.Instance.GetPlayerById(playerId).ModifyTurnIncome(cash);
        }

        public void RemoveCommand()
        {
            currentCommand = null;
        }

        public void AllowToBuildInThisTurn()
        {
            didBuildInThisTurn = false;
        }

        public void DestroyCity()
        {
            TurnManager.Instance.turnEvent -= AllowToBuildInThisTurn;
        }
    }
}