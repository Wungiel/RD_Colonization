namespace RD_Colonization.Code.Data
{
    public class UnitData
    {
        public string name;
        public bool land;
        public bool canBuild;

        public UnitData(string name, bool land, bool canBuild)
        {
            this.name = name;
            this.land = land;
            this.canBuild = canBuild;
        }
    }
}