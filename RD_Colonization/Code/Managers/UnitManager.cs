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

            TurnManager.turnEvent += moveUnits;
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


        public static bool checkPathfinding(Rectangle destiny)
        {
            Tile tmpTile = null;
            List<Tile> oldPath = null;
            movementDictionary.TryGetValue(currentUnit, out oldPath);
            movementDictionary.Remove(currentUnit);

            MapManager.mapDictionary.TryGetValue(destiny, out tmpTile);
            List<Tile> newPath = findPath(tmpTile);
            if (newPath.Count == 0)
            {
                Debug.WriteLine("False");
                movementDictionary.Add(currentUnit, oldPath);
                return false;
            }
            else
            {
                Debug.WriteLine("True");
                movementDictionary.Add(currentUnit, newPath);
                return true;
            }            
        }

        private static List<Tile> findPath(Tile tmpTile)
        {
            Tile nextTile = null;
            Vector2 distance = currentUnit.position.position.ToVector2() - tmpTile.position.ToVector2();
            int x = currentUnit.position.position.X;
            int y = currentUnit.position.position.Y;
            List<Tile> tmpPath = new List<Tile>();
            for (int i = 0; i <= distance.X; i++)
            {                
                Rectangle tmpRectangle = createRectangle(new Point(x, y));                
                MapManager.mapDictionary.TryGetValue(tmpRectangle, out nextTile);
                tmpPath.Add(nextTile);
                x--;
            }

            for (int i = 0; i <= distance.Y; i++)
            {                
                Rectangle tmpRectangle = createRectangle(new Point(x, y));
                MapManager.mapDictionary.TryGetValue(tmpRectangle, out nextTile);               
                tmpPath.Add(nextTile);
                y--;
            }

            return tmpPath;
        }

        private static void moveUnits()
        {
            foreach(KeyValuePair<Unit, List<Tile>> kvp in movementDictionary)
            {
                Unit tmpUnit = kvp.Key;
                List<Tile> tmpTiles = kvp.Value;
                if (tmpTiles.Count > 1)
                {
                    unitDictionary.Remove(createRectangle(tmpUnit.position));
                    tmpUnit.position = tmpTiles[1];
                    tmpTiles.RemoveAt(0);
                    unitDictionary.Add(createRectangle(tmpUnit.position), tmpUnit);
                }
            }
        }

        public static void destroyUnit(Unit unit)
        {
            if (unit == currentUnit)
                currentUnit = null;
            unitDictionary.Remove(unit.getPosition());
        }

        public static void changeCurrentUnit(Rectangle tempRectangle)
        {
            UnitManager.unitDictionary.TryGetValue(tempRectangle, out UnitManager.currentUnit);
        }

        public static void changeCurrentUnit()
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