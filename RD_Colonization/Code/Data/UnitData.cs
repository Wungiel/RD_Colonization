namespace RD_Colonization.Code.Data
{
    public class UnitData
    {
        public string name;
        public bool land;
        public bool canBuild;
        public bool canMoveUnits;
        public int fieldOfView;
        public int speed;
        public int maxHealth;
        public int strenght;
        public int cost;

        public UnitData(string name, bool land, bool canBuild, bool canMoveUnits, int speed, int health, int strenght, int cost)
        {
            this.name = name;
            this.land = land;
            this.canBuild = canBuild;
            this.canMoveUnits = canMoveUnits;
            this.fieldOfView = 2;
            this.speed = speed;
            this.maxHealth = health;
            this.strenght = strenght;
            this.cost = cost;
        }
    }
}