using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Managers
{
    internal static class MapManager
    {
        private static MapData mapData = null;

        public static void generateMap(int size)
        {
            mapData = new MapGenerator.generate(size);
        }
    }
}