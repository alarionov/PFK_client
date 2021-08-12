using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace WordBearers.ProfileScene
{
    public class PlayerInfo : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void jsGetBuffs(string address);
        
        [DllImport("__Internal")]
        private static extern void jsConductFight(string address);
        
        [DllImport("__Internal")]
        private static extern void jsGetCharacter(int id);
        
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        [SerializeField] private TMP_Text _tokenId;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _difficulty;

        private void Start()
        {
            Refresh(PlayerState.GetInstance());
            PlayerState.OnChange += Refresh;
        }

        private void OnDestroy()
        {
            PlayerState.OnChange -= Refresh;
        }

        private void Refresh(PlayerState state)
        {
            _tokenId?.SetText(state.State.tokenId.ToString());
            _level?.SetText(state.State.level.ToString());
            _difficulty?.SetText(state.State.difficulty.ToString());
        }

        public void LoadBuffs() => jsGetBuffs(PlayerState.GetInstance().Wallet);
        public void LoadCharacter() => jsGetCharacter(PlayerState.GetInstance().State.tokenId);
        public void ConductFight() => jsConductFight(PlayerState.GetInstance().Wallet);
        
    }
}
