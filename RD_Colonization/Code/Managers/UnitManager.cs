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
            typesDictionary.Add(civilianString, new UnitData(civilianString, true, true, 2));
            typesDictionary.Add(soldierString, new UnitData(soldierString, true, false, 1));
            typesDictionary.Add(scoutString, new UnitData(scoutString, true, false, 1));
            typesDictionary.Add(shipString, new UnitData(shipString, false, false, 1));
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
            UnitData type = GetUnitType(unitTypeString);
            Unit tmpUnit = new Unit(type, tile, playerId);
            List<Tile> tmpList = new List<Tile>();
            unitDictionary.Add(tile.CreateRectangle(), tmpUnit);
            movementDictionary.Add(tmpUnit, tmpList);
            DiscoverMap(tmpUnit);            
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

        public Unit[] GetPlayersUnits(int playerId)
        {
            List<Unit> playerUnits = new List<Unit>();
            foreach (Unit unit in unitDictionary.Values)
            {
                if (unit.playerId == playerId)
                {
                    playerUnits.Add(unit);
                }
            }
            return playerUnits.ToArray();
        }

        private void MoveUnits()
        {
            foreach (KeyValuePair<Unit, List<Tile>> kvp in movementDictionary)
            {
                if (unitDictionary.ContainsValue(kvp.Key))
                {
                    Unit tmpUnit = kvp.Key;
                    List<Tile> tmpTiles = kvp.Value;
                    while (tmpUnit.remainingEnergy > 0)
                    {
                        if (tmpTiles.Count > 0)
                        {
                            unitDictionary.Remove(tmpUnit.currentTile.CreateRectangle());
                            tmpUnit.currentTile = tmpTiles[0];
                            tmpTiles.RemoveAt(0);
                            unitDictionary.Add(tmpUnit.currentTile.CreateRectangle(), tmpUnit);
                            DiscoverMap(tmpUnit);
                            tmpUnit.remainingEnergy--;
                        } 
                        else
                        {
                            break;
                        }
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
                .Select(kv => kv.Value)
                .Where(u => u.playerId == PlayerManager.Instance.currentPlayer.id)
                .ToList();
            if (units.Count != 0)
            {
                int currentIndex = units.FindIndex(u => u == currentUnit);
                if (currentIndex == units.Count - 1)
                    currentIndex = 0;
                else
                    currentIndex++;
                currentUnit = units[currentIndex];
            }
            else
            {
                currentUnit = null;
            }
        }

        private void DiscoverMap(Unit tmpUnit)
        {
            MapManager.Instance.DiscoverMap(tmpUnit.currentTile, tmpUnit.type.fieldOfView, tmpUnit.playerId);
        }

    }
}