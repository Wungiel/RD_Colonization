﻿using Microsoft.Xna.Framework;
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

        public UnitManager()
        {
            typesDictionary.Add(civilianString, new UnitData(civilianString, true, true, 2, 1, 0));
            typesDictionary.Add(soldierString, new UnitData(soldierString, true, false, 1, 5, 2));
            typesDictionary.Add(scoutString, new UnitData(scoutString, true, false, 3, 1, 1));
            typesDictionary.Add(shipString, new UnitData(shipString, false, false, 1, 2, 2));
            TurnManager.Instance.turnEvent += MoveUnits;
        }
        
        public void AddNewBuildingUnit(PlayerData tmpPlayer, Tile tile)
        {
            SpawnUnit(tmpPlayer.id, tile, civilianString);
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

        public void DeselectUnit()
        {
            currentUnit = null;
        }

        public void ChangeCurrentUnit(Rectangle tempRectangle)
        {
            Unit tmpUnit = new Unit();
            unitDictionary.TryGetValue(tempRectangle, out tmpUnit);
            if (tmpUnit.playerId == PlayerManager.Instance.currentPlayer.id)
            {
                SetCurrentUnit(tmpUnit);
            }
        }

        public void ChangeCurrentUnit(PlayerData currentPlayer)
        {
            List<Unit> unitsList = unitDictionary.Values.ToList();
            for (int i = 0; i < unitsList.Count; i++)
            {
                if (unitsList[i].playerId == currentPlayer.id)
                {
                    SetCurrentUnit(unitsList[i]);
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
                int currentIndex = 0;
                if (currentUnit != null)
                {
                    currentIndex = units.FindIndex(u => u == currentUnit);
                    if (currentIndex == units.Count - 1)
                        currentIndex = 0;
                    else
                        currentIndex++;

                }

                SetCurrentUnit(units[currentIndex]);
            }
            else
            {
                currentUnit = null;
            }
        }

        private void SetCurrentUnit(Unit unit)
        {
            CityManager.Instance.DeselectCurrentCity();
            currentUnit = unit;
        }

        private void DiscoverMap(Unit tmpUnit)
        {
            MapManager.Instance.DiscoverMap(tmpUnit.currentTile, tmpUnit.type.fieldOfView, tmpUnit.playerId);
        }

    }
}