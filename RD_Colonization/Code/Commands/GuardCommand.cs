using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Commands
{
    public class GuardCommand : ICommand
    {
        public Unit attacker = null;
        public City cityGoal = null;
        public Unit unitGoal = null;
        public MoveCommand moveToDestinyTileCommand = null;

        public GuardCommand(Unit defender, City goal)
        {
            this.attacker = attacker;
            this.cityGoal = goal;
        }

        public GuardCommand(Unit defender, Unit goal)
        {
            this.attacker = attacker;
            this.unitGoal = goal;
        }


        //First get tiles of protected  object, stand close to it, if enemy is close then attack, if object destroyed then true
        public bool Execute()
        {
            if (cityGoal != null)
            {

            }
            else
            {

            }

            return false;

        }
    }
}
