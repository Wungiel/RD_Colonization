using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Commands
{
    class BuildCityCommand : ICommand
    {
        public Unit unitUnderCommand = null;

        public BuildCityCommand(Unit unit)
        {
            unitUnderCommand = unit;
        }

        public bool Execute()
        {

            ActionManager.Instance.BuildCity(unitUnderCommand);
            return true;
        }
    }
}
