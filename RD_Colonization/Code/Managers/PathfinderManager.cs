using Microsoft.Xna.Framework;
using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    public class PathfinderManager : BaseManager<PathfinderManager>
    {

        public bool CheckPathfinding(Rectangle destiny, MoveCommand command)
        {
            Unit unit = command.unit;
            Tile tmpTile = null;
            List<Tile> oldPath = command.GetPath();

            MapManager.Instance.mapDictionary.TryGetValue(destiny, out tmpTile);
            List<Tile> newPath = FindPath(tmpTile, unit);
            if (newPath == null)
            {
                command.SetPath(oldPath);
                return false;
            }
            else
            {
                command.SetPath(newPath);
                return true;
            }
        }

        public List<Tile> CheckPathfinding(Rectangle destiny, Unit unit, bool useUnitType)
        {
            return FindPath(MapManager.Instance.mapDictionary[destiny], unit, useUnitType);
        }

        private List<Tile> FindPath(Tile destinyTile, Unit unit, bool useUnitType = true)
        {
            List<Tile> closedSet = new List<Tile>();
            List<Tile> openset = new List<Tile>();
            Dictionary<Tile, float> g_score = new Dictionary<Tile, float>(); //Distance from starting point
            Dictionary<Tile, float> h_score = new Dictionary<Tile, float>(); //Distance from destination
            Dictionary<Tile, float> f_score = new Dictionary<Tile, float>(); //H+G
            Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>(); //Starting point
            Tile currentTile = null;

            openset.Add(unit.currentTile);
            g_score[unit.currentTile] = 0;

            while (openset.Count != 0)
            {
                currentTile = GetShortestOverallDistance(openset, destinyTile, g_score);
                if (currentTile == destinyTile)
                    return ReconstructPath(came_from, destinyTile);
                openset.Remove(currentTile);
                closedSet.Add(currentTile);
                foreach (Tile t in MapManager.Instance.GetNeighbours(currentTile))
                {
                    if (closedSet.Contains(t))
                        continue;
                    if (!t.type.walkable)
                    {
                        closedSet.Add(t);
                        continue;
                    }
                    if (OtherUnitToAvoid(t, unit) == true)
                    {
                        closedSet.Add(t);
                        continue;
                    }
                    if (CityToAvoid(t, unit) == true)
                    {
                        closedSet.Add(t);
                        continue;
                    }

                    if (useUnitType && (t.type.land ^ unit.type.land))
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
                    }
                    else if (temp_g < g_score[t])
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
            float min = CalculateDistance(openset[0], destinyTile) + g_score[closestTile];
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
            foreach (Tile t in openset)
            {
                float tmp = CalculateDistance(t, destinyTile);
                if (tmp < min)
                {
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

            double tmp = Math.Pow((startPoint.X - endPoint.X), 2) + Math.Pow((startPoint.Y - endPoint.Y), 2);
            tmp = Math.Sqrt(tmp);
            return (float)tmp;
        }

        private bool OtherUnitToAvoid(Tile t, Unit currentUnit)
        {
            if  (UnitManager.Instance.unitDictionary.ContainsKey(t.CreateRectangle()))
            {
                Unit tmpUnit = new Unit();
                UnitManager.Instance.unitDictionary.TryGetValue(t.CreateRectangle(), out tmpUnit);
                if (currentUnit.playerId == tmpUnit.playerId)
                {
                    return true;
                }
                else
                {
                    if (currentUnit.type.strenght == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            else
            {
                return false;
            }
        }

        private bool CityToAvoid(Tile t, Unit currentUnit)
        {
            if (CityManager.Instance.citytDictionary.ContainsKey(t.CreateRectangle()))
            {
                City tmpCity = new City();
                CityManager.Instance.citytDictionary.TryGetValue(t.CreateRectangle(), out tmpCity);
                if (currentUnit.playerId == tmpCity.playerId)
                {
                    return false;
                }
                else
                {
                    if (currentUnit.type.strenght == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            else
            {
                return false;
            }
        }


    }
}
