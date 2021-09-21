using System.Runtime.InteropServices;
using PFK;
using UnityEngine;
using UnityEngine.UI;
using WordBearers.Shared.Buffs;

#if UNITY_EDITOR
    using UnityEditor.Events;
#endif

namespace WordBearers.ProfileScene
{
    public class Spell : MonoBehaviour
    {
        private Button _castButton;

        [DllImport("__Internal")]
        private static extern void jsCastSpell(string address, int index, int wordCount);

        [SerializeField]
        private int _index;
        
        [SerializeField]
        private int _wordCount;

        public void Cast()
        {
            jsCastSpell(
                PlayerState.GetInstance().Wallet,
                _index,
                _wordCount);
        }
    }
}
