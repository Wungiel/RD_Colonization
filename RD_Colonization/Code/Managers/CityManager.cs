using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using System;
using System.Collections.Generic;

namespace RD_Colonization.Code.Managers
{
    public class CityManager : BaseManager<CityManager>
    {
        public Dictionary<Rectangle, City> citytDictionary = new Dictionary<Rectangle, City>();
        public City currentCity = null;

        public void SpawnCity(Unit unit)
        {
            City tmpCity = new City(unit.playerId, unit.currentTile);
            citytDictionary.Add(unit.GetPosition(), tmpCity);
            ScoreManager.Instance.AddNewCityPoint(unit.playerId);
            TurnManager.Instance.turnEvent += tmpCity.GenerateCash;
        }

        public City[] GetPlayersCities(int playerId)
        {

            List<City> playersCities = new List<City>();
            foreach (City city in citytDictionary.Values)
            {
                if (city.playerId == playerId)
                {
                    playersCities.Add(city);
                }
            }
            return playersCities.ToArray();
        }

        public void DeselectCurrentCity()
        {
            currentCity = null;
        }

        public void ChangeCurrenCity(Rectangle tempRectangle)
        {
            City tmpCity = new City();
            citytDictionary.TryGetValue(tempRectangle, out tmpCity);
            if (tmpCity.playerId == PlayerManager.Instance.currentPlayer.id)
            {
                SetCurrentCity(tmpCity);
            }
        }

        public bool CheckCitiesOnTiles(HashSet<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                if (citytDictionary.ContainsKey(t.CreateRectangle()))
                {
                    return true;
                }
            }
            return false;
        }

        public City GetCity(Rectangle tempRectangle)
        {
            City tmpCity = new City();
            citytDictionary.TryGetValue(tempRectangle, out tmpCity);
            return tmpCity;
        }

        public void DestroyCity(City city)
        {
            if (city == null)
            {
                return;
            }

            if (currentCity == city)
            {
                currentCity = null;
            }

            city.DestroyCity();
            TurnManager.Instance.turnEvent -= city.GenerateCash;
            citytDictionary.Remove(city.currentTile.CreateRectangle());
        }

        private void SetCurrentCity(City city)
        {
            UnitManager.Instance.DeselectUnit();
            currentCity = city;
        }

    }
}