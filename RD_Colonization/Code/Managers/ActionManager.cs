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

        public void CreateUnit(string unitKey)
        {
            if (CityManager.Instance.currentCity != null)
            {
                CreateUnit(CityManager.Instance.currentCity, unitKey);
            }                
        }

        public void CreateUnit(City city, string unitKey)
        {
            UnitData unitType = UnitManager.Instance.GetUnitType(unitKey);

            Tile tile = null;
            if (unitType.land == true)
            {
                tile = city.position.GetNeighbourTileForNewUnit();
            }
            else
            {
                tile = city.position.GetNeighbourTileForNewWaterUnit();
            }            

            if (tile != null)
            {
                UnitManager.Instance.AddNewUnit(PlayerManager.Instance.GetPlayerByCity(city), tile, unitKey);
            }            
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
