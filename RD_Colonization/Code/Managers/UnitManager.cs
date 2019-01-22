using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    class UnitManager
    {
        internal static Dictionary<Rectangle, Unit> unitDictionary;
        private static Dictionary<String, UnitData> typesDictionary = new Dictionary<String, UnitData>();
        public Unit currentUnit = null;

        static UnitManager()
        {
            typesDictionary.Add(civilianString, new UnitData(civilianString,true,true));
            typesDictionary.Add(soldierString, new UnitData(soldierString, true, false));
            typesDictionary.Add(shipString, new UnitData(shipString, false, false));
        }

        public static void setUpGameStart()
        {
            unitDictionary = new Dictionary<Rectangle, Unit>();
        }
    }
}
