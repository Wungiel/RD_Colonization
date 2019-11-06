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
    class UpgradeUnitCommand : ICommand
    {
        public City city;

        public UpgradeUnitCommand(City city)
        {
            this.city = city;
        }

        public bool Execute()
        {
            PlayerData player = PlayerManager.Instance.GetPlayerByCity(city);

            if (player.attackBonus > 1)
            {
                return true;
            }
            else
            {
                return ActionManager.Instance.UpgradeUnits(player);
            }
        }
    }
}
