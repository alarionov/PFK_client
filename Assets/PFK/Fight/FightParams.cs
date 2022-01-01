namespace PFK.Fight
{
    [System.Serializable]
    public class FightParams
    {
        public int Id;
        public string Seed;
        public BaseStats Character;
        public BaseStats[] Enemies;
        public bool Victory;
        public int Exp;
        public LevelUp[] LevelUps;
    }
}