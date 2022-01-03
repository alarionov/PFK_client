namespace PFK
{
    [System.Serializable]
    public class BaseStats
    {
        public enum AttributeType
        {
            Strength,
            Dexterity,
            Constitution,
            Luck,
            Armor,
            Attack,
            Health     
        }

        public int Strength;
        public int Dexterity;
        public int Constitution;
        
        public int Luck;
        public int Armour;
        
        public int Attack;
        public int Health;

        public bool Equals(BaseStats stats)
        {
            return 
                Strength == stats.Strength
                && Dexterity == stats.Dexterity
                && Constitution == stats.Constitution
                && Luck == stats.Luck
                && Armour == stats.Armour
                && Attack == stats.Attack 
                && Health == stats.Health;
        }
    }
}