using UnityEngine;

namespace PFK.Acts.Act001
{
    public class TestFight : MonoBehaviour
    {
        public void LoadFight(int index)
        {
            FightWrapper wrapper = new FightWrapper()
            {
                ContractAddress = "0x01",
                SceneIndex = index,
                FightParams = new FightParams()
            };
            
            WalletManager.GetInstance().FightScene(wrapper);
        }
    }
}
