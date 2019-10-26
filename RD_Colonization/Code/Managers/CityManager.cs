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
            City tmpCity = new City(unit.currentTile);
            citytDictionary.Add(unit.GetPosition(), tmpCity);
            TurnManager.Instance.turnEvent += tmpCity.GenerateCash;
        }
    }
}