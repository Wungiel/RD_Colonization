using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class UnitManager : BaseManager<UnitManager>
    {
        
        public Dictionary<Rectangle, Unit> unitDictionary = new Dictionary<Rectangle, Unit>();
        public Dictionary<Unit, List<Tile>> movementDictionary = new Dictionary<Unit, List<Tile>>();
        public Unit currentUnit = null;

        private Dictionary<String, UnitData> typesDictionary = new Dictionary<String, UnitData>();
        private static Random randomGenerator = new Random();
        

        public UnitManager()
        {
            typesDictionary.Add(civilianString, new UnitData(civilianString, true, true));
            typesDictionary.Add(soldierString, new UnitData(soldierString, true, false));
            typesDictionary.Add(shipString, new UnitData(shipString, false, false));
            TurnManager.Instance.turnEvent += MoveUnits;
        }
        
        public void AddNewBuildingUnit(PlayerData tmpPlayer)
        {
            var grassTiles = MapManager.Instance.mapDictionary
                .Where(kv => kv.Value.type.name == grassString).Select(kv => kv.Value).ToList();

            Tile tmpGrass = grassTiles[randomGenerator.Next(grassTiles.Count - 1)];

            SpawnUnit(tmpPlayer.id, tmpGrass, civilianString);
        }

        private void SpawnUnit(int playerId, Tile tile, String unitTypeString)
        {
            Unit tmpUnit = new Unit(GetUnitType(unitTypeString), tile, playerId);
            List<Tile> tmpList = new List<Tile>();
            unitDictionary.Add(tile.CreateRectangle(), tmpUnit);
            movementDictionary.Add(tmpUnit, tmpList);
        }

        public UnitData GetUnitType(String unitTypeString)
        {
            UnitData temp = null;
            typesDictionary.TryGetValue(unitTypeString, out temp);
            return temp;
        }

        public List<Tile> GetPathTiles(Unit unitKey)
        {
            List<Tile> temp = null;
            movementDictionary.TryGetValue(unitKey, out temp);
            return temp;
        }


        public bool CheckPathfinding(Rectangle destiny)
        {
            Tile tmpTile = null;
            List<Tile> oldPath = null;
            movementDictionary.TryGetValue(currentUnit, out oldPath);
            movementDictionary.Remove(currentUnit);

            MapManager.Instance.mapDictionary.TryGetValue(destiny, out tmpTile);
            List<Tile> newPath = FindPath(tmpTile);
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

        private List<Tile> FindPath(Tile destinyTile)
        {
            List<Tile> closedSet = new List<Tile>();
            List<Tile> openset = new List<Tile>();
            Dictionary<Tile, float> g_score = new Dictionary<Tile, float>(); //Distance from starting point
            Dictionary<Tile, float> h_score = new Dictionary<Tile, float>(); //Distance from destination
            Dictionary<Tile, float> f_score = new Dictionary<Tile, float>(); //H+G
            Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>(); //Skąd się przyszło
            Tile currentTile = null;

            openset.Add(currentUnit.currentTile);
            g_score[currentUnit.currentTile] = 0;

            while (openset.Count != 0)
            {
                currentTile = GetShortestOverallDistance(openset, destinyTile, g_score);
                if (currentTile == destinyTile)
                    return ReconstructPath(came_from, destinyTile);
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
                    if (t.type.land ^ currentUnit.type.land)
                    {
                        closedSet.Add(t);
                        continue;
                    }
                    float temp_g = g_score[currentTile] + 1;
                    if (!openset.Contains(t))
                    {
                        came_from[t] = currentTile;
                        openset.Add(t);
                        h_score[t] = CalculateDistance(t, destinyTile);
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

        private List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile currentNode)
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

        private Tile GetShortestOverallDistance(List<Tile> openset, Tile destinyTile, Dictionary<Tile, float> g_score)
        {
            Tile closestTile = openset[0];
            float min = CalculateDistance(openset[0], destinyTile)+ g_score[closestTile];
            foreach (Tile t in openset)
            {
                float tmp = CalculateDistance(t, destinyTile) + g_score[t];
                if (tmp < min)
                {
                    min = tmp;
                    closestTile = t;
                }
            }
            return closestTile;

        }

        private Tile GetShortestDistance(List<Tile> openset, Tile destinyTile)
        {
            Tile closestTile = openset[0];
            float min = CalculateDistance(openset[0], destinyTile);
            foreach(Tile t in openset)
            {
                float tmp = CalculateDistance(t, destinyTile);
                if (tmp < min){
                    min = tmp;
                    closestTile = t;
                }
            }
            return closestTile;
        }


        //Euclidean distance - units can move diagonally
        private float CalculateDistance(Tile start, Tile end)
        {
            Point startPoint = start.position;
            Point endPoint = end.position;

            double tmp = Math.Pow((startPoint.X-endPoint.X),2) + Math.Pow((startPoint.Y - endPoint.Y), 2);
            tmp = Math.Sqrt(tmp);
            return (float)tmp;
        }

        private void MoveUnits()
        {
            foreach (KeyValuePair<Unit, List<Tile>> kvp in movementDictionary)
            {
                if (unitDictionary.ContainsValue(kvp.Key))
                {
                    Unit tmpUnit = kvp.Key;
                    List<Tile> tmpTiles = kvp.Value;
                    if (tmpTiles.Count > 0)
                    {
                        unitDictionary.Remove(tmpUnit.currentTile.CreateRectangle());
                        tmpUnit.currentTile = tmpTiles[0];
                        tmpTiles.RemoveAt(0);
                        unitDictionary.Add(tmpUnit.currentTile.CreateRectangle(), tmpUnit);
                    }
                }
            }
        }

        public void DestroyUnit(Unit unit)
        {
            if (unit == currentUnit)
                currentUnit = null;
            unitDictionary.Remove(unit.GetPosition());
            movementDictionary.Remove(unit);
        }

        public void ChangeCurrentUnit(Rectangle tempRectangle)
        {
            UnitManager.Instance.unitDictionary.TryGetValue(tempRectangle, out UnitManager.Instance.currentUnit);
        }

        public void ChangeCurrentUnit(PlayerData currentPlayer)
        {
            List<Unit> unitsList = unitDictionary.Values.ToList();
            for (int i = 0; i < unitsList.Count; i++)
            {
                if (unitsList[i].playerId == currentPlayer.id)
                {
                    currentUnit = unitsList[i];
                    break;
                }
            }
        }


        public void ChangeCurrentUnit()
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