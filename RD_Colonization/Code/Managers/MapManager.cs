using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Managers
{
    static class MapManager
    {
        
        private static Dictionary<Rectangle, MapData> mapDictionary = null;

        public static void generateMap(int size)
        {
            MapData mapData = new MapGenerator().generate(size);
            mapDictionary = createDictionary(mapData);
        }

        public static void loadMap(MapData loadedData)
        {
            mapDictionary = createDictionary(loadedData);
        }

        private static Dictionary<Rectangle, MapData> createDictionary(MapData mapData)
        {
            throw new NotImplementedException();
        }
    }
}