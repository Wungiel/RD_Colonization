namespace RD_Colonization.Code.Data
{
    public class UnitData
    {
        public string name;
        public bool land;
        public bool canBuild;
        public int fieldOfView;
        public int speed;

        public UnitData(string name, bool land, bool canBuild, int speed)
        {
            this.name = name;
            this.land = land;
            this.canBuild = canBuild;
            this.fieldOfView = 2;
            this.speed = speed;
        }
    }
}