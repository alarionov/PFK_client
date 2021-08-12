using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace WordBearers.ProfileScene
{
    public class Buffs : MonoBehaviour
    {
        [SerializeField] private WordBearers.Shared.Buffs.Buff _statusPrefab;
        [SerializeField] private WordBearers.Shared.Buffs.Buff[] _buffs;

        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);

        private void Start()
        {
            foreach (WordBearers.Shared.Buffs.Buff buff in _buffs)
            {
                buff.SetState(false);
            }

            RenderBuffs(PlayerState.GetInstance());
            
            PlayerState.OnBuffsChange += RenderBuffs;
        }

        private void OnDestroy()
        {
            PlayerState.OnBuffsChange -= RenderBuffs;
        }

        private void RenderBuffs(PlayerState playerState)
        {
            for (int i = 0; i < playerState.Buffs.Length; i++)
            {
                _buffs[i].SetState(playerState.Buffs[i]);
                _buffs[i].gameObject.GetComponentInChildren<Button>().interactable = !playerState.Buffs[i];
            }
        }
    }
}
