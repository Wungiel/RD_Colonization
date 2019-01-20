using RD_Colonization.Code.Data;

namespace RD_Colonization.Code.Managers
{
    internal class MapGenerator : MapData
    {
        private int size;

        public MapGenerator(int size)
        {
            this.size = size;
        }

        internal class generate : MapData
        {
            private int size;

            public generate(int size)
            {
                this.size = size;
            }
        }
    }
}