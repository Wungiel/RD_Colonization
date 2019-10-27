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
            if (CanBuild() == true)
            {
                CityManager.Instance.SpawnCity(UnitManager.Instance.currentUnit);
                UnitManager.Instance.DestroyUnit(UnitManager.Instance.currentUnit);
            }
        }

        private bool CanBuild()
        {
            return UnitManager.Instance.currentUnit != null && UnitManager.Instance.currentUnit.type.canBuild;
        }
    }
}
