using UnityEngine;

namespace PFK.Acts.Act001
{
    public class TestFight : MonoBehaviour
    {
        [SerializeField] private string _contractAddress;
        [SerializeField] private int _index;
        public void LoadFight()
        {
            FightWrapper wrapper = new FightWrapper()
            {
                ContractAddress = _contractAddress,
                SceneIndex = _index,
                FightParams = new FightParams()
            };
            
            WalletManager.GetInstance().FightScene(wrapper);
        }
    }
}
