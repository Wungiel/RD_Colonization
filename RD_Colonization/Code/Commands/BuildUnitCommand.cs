using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Commands
{
    public class BuildUnitCommand : ICommand
    {
        City buildingCity;
        String unitTypeString;

        public BuildUnitCommand(City buildingCity, String unitTypeString)
        {
            this.buildingCity = buildingCity;
            this.unitTypeString = unitTypeString;
        }

        public bool Execute()
        {
            if (UnitManager.Instance.GetUnitType(unitTypeString) == null)
            {
                return true;
            }
            else
            {
                return ActionManager.Instance.CreateUnit(buildingCity, unitTypeString);
            }
        }
    }
}
