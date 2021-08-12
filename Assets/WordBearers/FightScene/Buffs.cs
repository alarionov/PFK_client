using UnityEngine;
using WordBearers.Shared.Buffs;

namespace WordBearers.FightScene
{
    public class Buffs : MonoBehaviour
    {
        [SerializeField] private Buff[] _buffs;

        private void Start()
        {
            bool[] buffs = Fight.Instance.FightParams.Buffs;
            
            for (int i = 0; i < buffs.Length; i++)
            {
                _buffs[i].SetState(buffs[i]);
            }
        }
    }
}
