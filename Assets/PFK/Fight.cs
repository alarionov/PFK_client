using UnityEngine;

namespace PFK
{
    public class Fight
    {
        public static Fight Instance { get; private set; }
        
        public FightParams FightParams { get; private set; }

        public static void NewFight(FightParams fightParams)
        {
            Instance = new Fight {FightParams = fightParams};
        }

        public static void LoadParams(string encodedFight)
        {
            FightParams fightParams = JsonUtility.FromJson<FightParams>(encodedFight);
            NewFight(fightParams);
        }
    }
}
