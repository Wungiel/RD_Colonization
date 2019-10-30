﻿using RD_Colonization.Code.Data;
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

        public void CreateUnit(string unitKey)
        {
            if (CityManager.Instance.currentCity != null)
            {
                CreateUnit(CityManager.Instance.currentCity, unitKey);
            }                
        }

        public void CreateUnit(City city, string unitKey)
        {
            if (city.didBuildInThisTurn == true)
            {
                return;
            }

            UnitData unitType = UnitManager.Instance.GetUnitType(unitKey);

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
                PlayerData player = PlayerManager.Instance.GetPlayerByCity(city);
                UnitManager.Instance.AddNewUnit(player, tile, unitKey);
                city.didBuildInThisTurn = true;
                ScoreManager.Instance.AddNewUnitPoint(player.id);
            }            
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
