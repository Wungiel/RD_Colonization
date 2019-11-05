using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Commands
{
    public class AttackCommand : ICommand
    {
        public Unit attacker = null;
        public City cityGoal = null;
        public Unit unitGoal = null;
        public MoveCommand moveToDestinyTileCommand = null;

        public AttackCommand(Unit attacker, City goal)
        {
            this.attacker = attacker;
            this.cityGoal = goal;
        }

        public AttackCommand(Unit attacker, Unit goal)
        {
            this.attacker = attacker;
            this.unitGoal = goal;
        }

        public bool Execute()
        {
            Tile destinyTile = null;

            if (unitGoal == null)
            {
                if (CityManager.Instance.citytDictionary.ContainsValue(cityGoal) == false)
                {
                    return true;
                }

                destinyTile = cityGoal.currentTile;
            }
            else
            {
                if (UnitManager.Instance.unitDictionary.ContainsValue(unitGoal) == false)
                {
                    return true;
                }
                destinyTile = unitGoal.currentTile;
            }

            if (attacker.currentTile != destinyTile)
            {
                if (moveToDestinyTileCommand == null)
                {
                    moveToDestinyTileCommand = new MoveCommand(destinyTile, attacker);
                }

                moveToDestinyTileCommand.Execute();
                return false;
            }

            return true;

        }
    }
}
