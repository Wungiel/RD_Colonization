namespace RD_Colonization.Code.Data
{
    public class MapData
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