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
    class ActionManager : BaseManager<ActionManager>
    {
        public void BuildCity()
        {
            if (UnitManager.Instance.currentUnit != null)
            {
                BuildCity(UnitManager.Instance.currentUnit);
            }            
        }

        public void BuildCity(Unit unit)
        {
            if (CanBuild(unit) == true)
            {
                CityManager.Instance.SpawnCity(unit);
                UnitManager.Instance.DestroyUnit(unit);
            }
        }

        public bool CreateUnit(string unitKey)
        {
            if (CityManager.Instance.currentCity != null)
            {
                return CreateUnit(CityManager.Instance.currentCity, unitKey);
            }          
            else
            {
                return false;
            }
        }

        public bool CreateUnit(City city, string unitKey)
        {
            if (city.didBuildInThisTurn == true)
            {
                return false;
            }

            UnitData unitType = UnitManager.Instance.GetUnitType(unitKey);
            PlayerData player = PlayerManager.Instance.GetPlayerByCity(city);

            if (player.ModifyCashPayment(unitType.cost) == false)
            {
                return false;
            }

            Tile tile = null;
            if (unitType.land == true)
            {
                tile = city.currentTile.GetNeighbourTileForNewUnit();
            }
            else
            {
                tile = city.currentTile.GetNeighbourTileForNewWaterUnit();
            }            

            if (tile != null)
            {                
                UnitManager.Instance.AddNewUnit(player, tile, unitKey);
                city.didBuildInThisTurn = true;
                ScoreManager.Instance.AddNewUnitPoint(player.id);
                return true;
            }

            return false;
        }

        public void DestroyCity(Unit attacker, City defender)
        {
            CityManager.Instance.DestroyCity(defender);
            ScoreManager.Instance.AddDestroyedCityPoint(attacker.playerId);
        }

        public void StartBattle(Unit attacker, Unit defender)
        {
            bool fightFinished = false;
            int turnCount = 0;

            while (fightFinished == false)
            {
                attacker.health -= GetAttackPower(defender);
                if (attacker.health > 0)
                {
                    defender.health -= GetAttackPower(attacker);
                    if (defender.health < 0)
                    {
                        fightFinished = true;
                        UnitManager.Instance.DestroyUnit(defender);
                        ScoreManager.Instance.AddDestroyedUnitPoint(attacker.playerId);
                    }
                } 
                else
                {                    
                    fightFinished = true;
                    UnitManager.Instance.DestroyUnit(attacker);
                    ScoreManager.Instance.AddDestroyedUnitPoint(defender.playerId);
                }

                if (turnCount == 100)
                {
                    fightFinished = true;
                }

                turnCount++;
            }
        }

        public void LoadUnit(Unit cargo, Unit transport)
        {
            if (transport.type.canMoveUnits == true && transport.transportedUnit == null && cargo.type.canMoveUnits == false && cargo.remainingEnergy != 0)
            {
                if (cargo.currentTile.neighbours.Contains(transport.currentTile))
                {
                    transport.transportedUnit = cargo;
                    UnitManager.Instance.DestroyUnit(cargo);
                }
            }
        }

        public void UnloadUnit(Unit transport, Tile destination)
        {
            if (transport.type.canMoveUnits == true && transport.transportedUnit != null && transport.remainingEnergy != 0)
            {
                if (!(destination.type.land ^ transport.transportedUnit.type.land) && transport.currentTile.neighbours.Contains(destination))
                {
                    transport.transportedUnit.currentTile = destination;
                    UnitManager.Instance.unitDictionary.Add(destination.CreateRectangle(), transport.transportedUnit);
                    transport.transportedUnit.remainingEnergy = 0;
                    transport.remainingEnergy = 0;
                    transport.transportedUnit = null;
                }
            }

        }

        private float GetAttackPower(Unit attackingUnit)
        {
            PlayerData unitOwner = PlayerManager.Instance.GetPlayerByUnit(attackingUnit);
            return attackingUnit.type.strenght * unitOwner.GetDDABonus() * unitOwner.GetBoughtBonus();
        }

        private bool CanBuild(Unit unit)
        {
            bool canUnitBuild = unit != null && unit.type.canBuild;
            bool isTileAcceptable = CityManager.Instance.citytDictionary.ContainsKey(unit.GetPosition()) == false;
            bool isFarFromOtherCities = CityManager.Instance.CheckCitiesOnTiles(MapManager.Instance.GetNeighbours(unit.currentTile, 2)) == false;
            return canUnitBuild  && isTileAcceptable && isFarFromOtherCities;
        }
    }
}
