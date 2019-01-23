using Microsoft.Xna.Framework;
using RD_Colonization.Code.Entities;
using System.Collections.Generic;

namespace RD_Colonization.Code.Managers
{
    public static class CityManager
    {
        public static Dictionary<Rectangle, City> citytDictionary = new Dictionary<Rectangle, City>();

        public static void spawnCity(Unit unit)
        {
            citytDictionary.Add(unit.getPosition(), new City(unit.position));
        }
    }
}