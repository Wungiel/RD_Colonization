namespace RD_Colonization.Code.Data
{
    public class UnitData
    {
        public string name;
        public bool land;
        public bool canBuild;
        public int fieldOfView;
        public int speed;
        public int maxHealth;
        public int strenght;

        public UnitData(string name, bool land, bool canBuild, int speed, int health, int strenght)
        {
            this.name = name;
            this.land = land;
            this.canBuild = canBuild;
            this.fieldOfView = 2;
            this.speed = speed;
            this.maxHealth = health;
            this.strenght = strenght;
        }
    }
}