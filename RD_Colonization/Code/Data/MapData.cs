using System.Collections.Generic;

namespace RD_Colonization.Code.Data
{
    internal class MapData
    {
        public float[,] tileArray;
        public static LinkedList<Tile> tiles;

        public MapData(float[,] temp)
        {
            this.tileArray = temp;
        }
    }
}