namespace WordBearers
{
    [System.Serializable]
    public class BaseStats
    {
        public int Attack;
        public int Health;
        public int Armour;

        public bool Equals(BaseStats stats)
        {
            return Attack == stats.Attack && Health == stats.Health && Armour == stats.Armour;
        }

        public static BaseStats ApplyBuffs(BaseStats stats, bool[] buffs, bool copy = false)
        {
            BaseStats buffedStats = 
                !copy
                    ? stats
                    : new BaseStats()
                    {
                        Attack = stats.Attack, 
                        Health = stats.Health, 
                        Armour = stats.Armour
                    };
            
            if (buffs[(int)SpellType.Enchant])
            {
                buffedStats.Attack += 1;
            }
        
            if (buffs[(int)SpellType.FalseLife])
            {
                buffedStats.Health += 3;
            }
        
            if (buffs[(int)SpellType.Shield])
            {
                buffedStats.Armour += 1;
            }
        
            if (buffs[(int)SpellType.Bless])
            {
                buffedStats.Attack += 1;
                buffedStats.Health += 3;
                buffedStats.Armour += 1;
            }

            return buffedStats;
        }
    }
}