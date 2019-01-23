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
            TurnManager.turnEvent += moveUnits;
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

            spawnUnit(tmpGrass, civilianString);
            spawnUnit(tmpWater, shipString);
            unitDictionary.TryGetValue(createRectangle(tmpGrass), out currentUnit);
            
        }

        private static void spawnUnit(Tile tile, String key)
        {
            Unit tmpUnit = new Unit(getUnitType(key), tile);
            List<Tile> tmpList = new List<Tile>();
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
            if (newPath == null)
            {
                movementDictionary.Add(currentUnit, oldPath);
                return false;
            }
            else
            {
                movementDictionary.Add(currentUnit, newPath);
                return true;
            }   
        }

        private static List<Tile> findPath(Tile destinyTile)
        {
            List<Tile> closedSet = new List<Tile>();
            List<Tile> openset = new List<Tile>();
            Dictionary<Tile, float> g_score = new Dictionary<Tile, float>(); //Distance from starting point
            Dictionary<Tile, float> h_score = new Dictionary<Tile, float>(); //Distance from destination
            Dictionary<Tile, float> f_score = new Dictionary<Tile, float>(); //H+G
            Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>(); //Skąd się przyszło
            Tile currentTile = null;

            openset.Add(currentUnit.position);
            g_score[currentUnit.position] = 0;

            while (openset.Count != 0)
            {                
                currentTile = getShortestOverallDistance(openset, destinyTile, g_score);
                if (currentTile == destinyTile)
                    return reconstructPath(came_from, destinyTile);
                openset.Remove(currentTile);
                closedSet.Add(currentTile);
                foreach (Tile t in currentTile.neighbours)
                {
                    if (closedSet.Contains(t))
                        continue;
                    if (!t.type.walkable)
                    {
                        closedSet.Add(t);
                        continue;
                    }
                    if ((t.type.land && !currentUnit.type.land) || (!t.type.land && currentUnit.type.land))
                    {
                        closedSet.Add(t);
                        continue;
                    }
                    float temp_g = g_score[currentTile] + 1;                    
                    if (!openset.Contains(t))
                    {
                        came_from[t] = currentTile;
                        openset.Add(t);
                        h_score[t] = calculateDistance(t, destinyTile);
                        g_score[t] = temp_g;
                    } else if (temp_g < g_score[t])
                    {
                        came_from[t] = currentTile;
                        g_score[t] = temp_g;
                        f_score[t] = g_score[t] + h_score[t];
                    }
                }             
            }
            return null;
        }

        private static List<Tile> reconstructPath(Dictionary<Tile, Tile> cameFrom, Tile currentNode)
        {
            List<Tile> tmpList = new List<Tile>();
            Tile tmpTile = null;
            while (cameFrom.TryGetValue(currentNode, out tmpTile))
            {
                tmpList.Add(currentNode);
                currentNode = tmpTile;
            }
            tmpList.Reverse();
            return tmpList;
        }

        private static Tile getShortestOverallDistance(List<Tile> openset, Tile destinyTile, Dictionary<Tile, float> g_score)
        {
            Tile closestTile = openset[0];
            float min = calculateDistance(openset[0], destinyTile)+ g_score[closestTile];
            foreach (Tile t in openset)
            {
                float tmp = calculateDistance(t, destinyTile) + g_score[t];
                if (tmp < min)
                {
                    min = tmp;
                    closestTile = t;
                }
            }
            return closestTile;

        }

        private static Tile getShortestDistance(List<Tile> openset, Tile destinyTile)
        {
            Tile closestTile = openset[0];
            float min = calculateDistance(openset[0], destinyTile);
            foreach(Tile t in openset)
            {
                float tmp = calculateDistance(t, destinyTile);
                if (tmp < min){
                    min = tmp;
                    closestTile = t;
                }
            }
            return closestTile;
        }


        //Euclidean distance - units can move diagonally
        private static float calculateDistance(Tile start, Tile end)
        {
            Point startPoint = start.position;
            Point endPoint = end.position;

            double tmp = Math.Pow((startPoint.X-endPoint.X),2) + Math.Pow((startPoint.Y - endPoint.Y), 2);
            tmp = Math.Sqrt(tmp);
            return (float)tmp;
        }

        private static void moveUnits()
        {
            foreach (KeyValuePair<Unit, List<Tile>> kvp in movementDictionary)
            {
                if (unitDictionary.ContainsValue(kvp.Key))
                {
                    Unit tmpUnit = kvp.Key;
                    List<Tile> tmpTiles = kvp.Value;
                    if (tmpTiles.Count > 0)
                    {
                        unitDictionary.Remove(createRectangle(tmpUnit.position));
                        tmpUnit.position = tmpTiles[0];
                        tmpTiles.RemoveAt(0);
                        unitDictionary.Add(createRectangle(tmpUnit.position), tmpUnit);
                    }
                }
                else movementDictionary.Remove(kvp.Key);
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