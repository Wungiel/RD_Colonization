using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static RD_Colonization.Code.StringList;
using static RD_Colonization.Code.RectangleHelper;

namespace RD_Colonization.Code.Managers
{
    public static class UnitManager
    {
        public static Dictionary<Rectangle, Unit> unitDictionary;
        public static Dictionary<Unit, List<Tile>> movementDictionary = new Dictionary<Unit, List<Tile>>();
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
                .Where(kv => kv.Value.type.name == grassString).Select(kv => kv.Value).ToList();

            var waterTiles = MapManager.mapDictionary
                .Where(kv => kv.Value.type.name == waterString).Select(kv => kv.Value).ToList();

            Tile tmpGrass = grassTiles[new Random().Next(grassTiles.Count() - 1)];
            Tile tmpWater = waterTiles[new Random().Next(waterTiles.Count() - 1)];

            Debug.WriteLine(tmpGrass.position);
            Debug.WriteLine(tmpWater.position);

            spawnUnit(tmpGrass, civilianString);
            spawnUnit(tmpWater, shipString);
            unitDictionary.TryGetValue(createRectangle(tmpGrass), out currentUnit);
        }

        private static void spawnUnit(Tile tile, String key)
        {
            Unit tmpUnit = new Unit(getUnitType(key), tile);
            List<Tile> tmpList = new List<Tile>();
            tmpList.Add(tile);
            unitDictionary.Add(createRectangle(tile), tmpUnit);
            movementDictionary.Add(tmpUnit, tmpList);
        }

        public static UnitData getUnitType(String key)
        {
            UnitData temp = null;
            typesDictionary.TryGetValue(key, out temp);
            return temp;
        }

        public static List<Tile> getPathTiles(Unit key)
        {
            List<Tile> temp = null;
            movementDictionary.TryGetValue(key, out temp);
            return temp;
        }


        public static bool checkPathfinding()
        {
            findPath();
            return true;
        }

        private static void findPath()
        {
            throw new NotImplementedException();
        }

        internal static void changeCurrentUnit(Rectangle tempRectangle)
        {
            UnitManager.unitDictionary.TryGetValue(tempRectangle, out UnitManager.currentUnit);
        }

        internal static void changeCurrentUnit()
        {
            var units = unitDictionary
                .Select(kv => kv.Value).ToList();
            int currentIndex = units.FindIndex(u => u == currentUnit);
            if (currentIndex == units.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;
            currentUnit = units[currentIndex];
        }
    }
}