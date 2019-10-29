using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    class ActionManager : BaseManager<ActionManager>
    {
        public void BuildCity()
        {
            BuildCity(UnitManager.Instance.currentUnit);
        }

        public void BuildCity(Unit unit)
        {
            if (CanBuild(unit) == true)
            {
                CityManager.Instance.SpawnCity(unit);
                UnitManager.Instance.DestroyUnit(unit);
            }
        }

        public void CreateUnit()
        {
            if (CityManager.Instance.currentCity != null)
                CreateUnit(CityManager.Instance.currentCity);
        }

        public void CreateUnit(City city)
        {
            Tile tile = city.position.GetNeighbourTileForNewUnit();
            if (tile != null)
            {
                UnitManager.Instance.AddNewBuildingUnit(PlayerManager.Instance.GetPlayerByCity(city), tile);
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
