using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    class ActionManager : BaseManager<ActionManager>
    {
        public void BuildCity()
        {
            BuildCity(UnitManager.Instance.currentUnit);
        }

        public void BuildCity(Unit unit)
        {
            if (CanBuild(unit) == true)
            {
                CityManager.Instance.SpawnCity(unit);
                UnitManager.Instance.DestroyUnit(unit);
            }
        }

        private bool CanBuild(Unit unit)
        {
            return unit != null && unit.type.canBuild;
        }
    }
}
