using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public static class UnitManager
    {
        public static Dictionary<Rectangle, Unit> unitDictionary;
        private static Dictionary<String, UnitData> typesDictionary = new Dictionary<String, UnitData>();
        public static Unit currentUnit = null;

        static UnitManager()
        {
            typesDictionary.Add(civilianString, new UnitData(civilianString, true, true));
            typesDictionary.Add(soldierString, new UnitData(soldierString, true, false));
            typesDictionary.Add(shipString, new UnitData(shipString, false, false));
        }

        public static void setUpGameStart()
        {
            unitDictionary = new Dictionary<Rectangle, Unit>();

            var grassTiles = MapManager.mapDictionary
                .Where(kv => kv.Value.type.name == grassString).Select(kv => kv.Value);

            var waterTiles = MapManager.mapDictionary
                .Where(kv => kv.Value.type.name == waterString).Select(kv => kv.Value);

            foreach (Tile t in grassTiles)
            {

            }
        }
    }
}