using System.Linq;
using UnityEngine;

namespace PFK.Test
{
    public class TestFight : MonoBehaviour
    {
        [SerializeField] private string _contractAddress;
        [SerializeField] private int _index;
        [SerializeField] private int _level;
        
        public void LoadFight()
        {
            Fight.FightWrapper wrapper = new Fight.FightWrapper()
            {
                ContractAddress = _contractAddress,
                SceneIndex = _index,
                FightParams = new Fight.FightParams()
                {
                    Seed = GenerateRandomSeed(64*4),
                    Score = 1,
                    Stats = new BaseStats(){ Attack = 1, Health = 3, Armour = 1},
                    Buffs = new[]{false, false, false, false, false, false, false, false},
                    Victory = true,
                    Died = false,
                    LevelUps = new LevelUp[]{}
                }
            };
            
            WalletManager.GetInstance().FightScene(wrapper);
        }

        private static string GenerateRandomSeed(int length)
        {
            const string chars = "ABCDEF0123456789";
 
           System.Random random = new System.Random();
            string randomString = 
                new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            
            return randomString;
        }
    }
}
