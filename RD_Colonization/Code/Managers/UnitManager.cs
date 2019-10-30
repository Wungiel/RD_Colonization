using Microsoft.Xna.Framework;
using RD_Colonization.Code.Commands;
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
        public Unit currentUnit = null;

        private Dictionary<String, UnitData> typesDictionary = new Dictionary<String, UnitData>();      

        public UnitManager()
        {
            typesDictionary.Add(civilianString, new UnitData(civilianString, true, true, 2, 1, 0));
            typesDictionary.Add(soldierString, new UnitData(soldierString, true, false, 1, 5, 2));
            typesDictionary.Add(scoutString, new UnitData(scoutString, true, false, 5, 1, 1));
            typesDictionary.Add(shipString, new UnitData(shipString, false, false, 1, 2, 2));
        }
        
        public void AddNewUnit(PlayerData tmpPlayer, Tile tile, String unitType)
        {
            SpawnUnit(tmpPlayer.id, tile, unitType);
        }

        private void SpawnUnit(int playerId, Tile tile, String unitTypeString)
        {
            UnitData type = GetUnitType(unitTypeString);
            Unit tmpUnit = new Unit(type, tile, playerId);
            List<Tile> tmpList = new List<Tile>();
            unitDictionary.Add(tile.CreateRectangle(), tmpUnit);
            DiscoverMap(tmpUnit);            
        }

        public void ChangeUnitPlace(Unit unit, Tile tile)
        {
            unitDictionary.Remove(unit.currentTile.CreateRectangle());
            unitDictionary.Add(tile.CreateRectangle(), unit);
            unit.currentTile = tile;
        }

        public UnitData GetUnitType(String unitTypeString)
        {
            UnitData temp = null;
            typesDictionary.TryGetValue(unitTypeString, out temp);
            return temp;
        }

        public List<Tile> GetPathTiles(Unit unitKey)
        {
            MoveCommand command = (MoveCommand) unitKey.currentCommand;
            if (command != null)
            {
                return command.GetPath();
            }
            else
            {
                return new List<Tile>();
            }
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

        public void CheckBattle(Unit movedUnit, Tile tile)
        {
            Rectangle tileRectangle = tile.CreateRectangle();

            if (UnitManager.Instance.unitDictionary.ContainsKey(tileRectangle))
            {
                Unit tmpUnit = new Unit();
                UnitManager.Instance.unitDictionary.TryGetValue(tileRectangle, out tmpUnit);
                if (tmpUnit.playerId != movedUnit.playerId)
                {
                    ActionManager.Instance.StartBattle(movedUnit, tmpUnit);
                }
            } 
            else if(CityManager.Instance.citytDictionary.ContainsKey(tileRectangle))
            {
                City tmpCity = new City();
                CityManager.Instance.citytDictionary.TryGetValue(tileRectangle, out tmpCity);
                if (tmpCity.playerId != movedUnit.playerId)
                {
                    ActionManager.Instance.DestroyCity(movedUnit, tmpCity);
                }

            }
        }

        public void DestroyUnit(Unit unit)
        {
            if (unit == currentUnit)
                currentUnit = null;
            unitDictionary.Remove(unit.GetPosition());
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

        public void DiscoverMap(Unit tmpUnit)
        {
            MapManager.Instance.DiscoverMap(tmpUnit.currentTile, tmpUnit.type.fieldOfView, tmpUnit.playerId);
        }

        public bool IsUnitOnRectangleFriendly(Rectangle tempRectangle)
        {
            return IsUnitOnRectangleFriendly(tempRectangle, PlayerManager.Instance.currentPlayer.id);
        }

        public bool IsUnitOnRectangleFriendly(Rectangle tempRectangle, int playerId)
        {
            Unit otherUnit = new Unit();
            unitDictionary.TryGetValue(tempRectangle, out otherUnit);
            if (playerId == otherUnit.playerId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Unit[] CollectEnemyUnitsFromTiles(Tile[] discoveredTiles, int playerId)
        {
            List<Unit> enemyUnits = new List<Unit>();

            foreach (Tile t in discoveredTiles)
            {
                Rectangle tileRectangle = t.CreateRectangle();
                if (unitDictionary.ContainsKey(t.CreateRectangle()))
                {
                    if (unitDictionary[tileRectangle].playerId != playerId)
                    {
                        enemyUnits.Add(unitDictionary[tileRectangle]);
                    }
                }
            }

            return enemyUnits.ToArray();
        }


        private void SetCurrentUnit(Unit unit)
        {
            CityManager.Instance.DeselectCurrentCity();
            currentUnit = unit;
        }

    }
}