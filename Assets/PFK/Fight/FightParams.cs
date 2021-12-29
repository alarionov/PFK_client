namespace PFK.Fight
{
    [System.Serializable]
    public class FightParams
    {
        public int Id;
        public int Season;
        public string Seed;
        public int Score;
        public BaseStats Stats;
        public bool[] Buffs;
        public bool Victory;
        public bool Died;
        public LevelUp[] LevelUps;
    }
}