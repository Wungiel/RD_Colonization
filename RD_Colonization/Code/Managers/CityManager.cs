using Microsoft.Xna.Framework;
using RD_Colonization.Code.Entities;
using System.Collections.Generic;

namespace RD_Colonization.Code.Managers
{
    public class CityManager : BaseManager<CityManager>
    {
        public Dictionary<Rectangle, City> citytDictionary = new Dictionary<Rectangle, City>();

        public void SpawnCity(Unit unit)
        {
            City tmpCity = new City(unit.playerId, unit.currentTile);
            citytDictionary.Add(unit.GetPosition(), tmpCity);
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
    }
}