using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PFK.GameMenu.CharacterSelect
{
    public class AddTokenButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _tokenID;
        [SerializeField] private Button _button;

        [DllImport("__Internal")]
        private static extern void jsAddPurr(string wallet, int token);
        
        private void Start()
        {
            SetButton();
        }

        public void SetButton()
        {
            bool success = int.TryParse(_tokenID.text, out int token);
            _button.interactable = success && token is > 0 and <= 10000;
        }

        public void AddToken()
        {
            Debug.Log($"token id is {int.Parse(_tokenID.text)}");
            jsAddPurr(PlayerState.GetInstance().Wallet, int.Parse(_tokenID.text));
        }
    }
}
