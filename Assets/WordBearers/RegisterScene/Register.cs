using System.Runtime.InteropServices;
using UnityEngine;

namespace WordBearers.RegisterScene
{
    public class Register : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void jsRegisterCharacter(string address);
        
        public void RegisterNewCharacter()
        {
            jsRegisterCharacter(PlayerState.GetInstance().Wallet);
        }
    }
}
