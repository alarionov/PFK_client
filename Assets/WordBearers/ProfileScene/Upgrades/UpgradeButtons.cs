using System.Runtime.InteropServices;
using PFK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WordBearers.ProfileScene.Upgrades
{
    public class UpgradeButtons : MonoBehaviour
    {
        [SerializeField] private TMP_Text _upgradeCount;

        [Header("Upgrade Buttons")] 
        [SerializeField] private Button _upgradeAttack;
        [SerializeField] private Button _upgradeHealth;
        [SerializeField] private Button _upgradeArmour;

        [DllImport("__Internal")]
        private static extern void jsUpgradeAttack(string address, int id);
        
        [DllImport("__Internal")]
        private static extern void jsUpgradeHealth(string address, int id);
        
        [DllImport("__Internal")]
        private static extern void jsUpgradeArmour(string address, int id);
        
        private void Start()
        {
            _upgradeArmour.interactable = false;
            _upgradeAttack.interactable = false;
            _upgradeHealth.interactable = false;
            
            Render(PlayerState.GetInstance());
            
            PlayerState.OnCharacterChange += Render;
        }

        private void OnDestroy()
        {
            PlayerState.OnCharacterChange -= Render;
        }

        private void Render(PlayerState state)
        {
            _upgradeCount?.SetText(state.Character.Upgrades.ToString());
            SetButtonState(state.Character.Upgrades > 0);
        }

        public void SetButtonState(bool state)
        {
            _upgradeArmour.interactable = state;
            _upgradeAttack.interactable = state;
            _upgradeHealth.interactable = state;
        }

        public void UpgradeAttack() => 
            jsUpgradeAttack(PlayerState.GetInstance().Wallet, PlayerState.GetInstance().State.tokenId);

        public void UpgradeHealth() => 
            jsUpgradeHealth(PlayerState.GetInstance().Wallet, PlayerState.GetInstance().State.tokenId);

        public void UpgradeArmour() => 
            jsUpgradeArmour(PlayerState.GetInstance().Wallet, PlayerState.GetInstance().State.tokenId);
    }
}
