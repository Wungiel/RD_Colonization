using System.Collections.Generic;

namespace RD_Colonization.Code.Data
{
    internal class MapData
    {
        public string tileData;
        public int size;

        public MapData(int size, string tileData)
        {
            this.size = size;
            this.tileData = tileData;
        }
    }
}