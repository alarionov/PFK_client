using System.Runtime.InteropServices;
using UnityEngine;

namespace PFK.GameMenu.CharacterSelect
{
    public class ChooseButton : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void jsGetCharacter(string address, int token);
        
        [DllImport("__Internal")]
        private static extern void jsGetAct1Progress(string address, int token);

        public void Choose()
        {
        }

        public void Choose(int token)
        {
            jsGetCharacter("0x8D5e0fA8b10D65E3eedF6B080Add24F45818b93E", token);
            jsGetAct1Progress("0x8D5e0fA8b10D65E3eedF6B080Add24F45818b93E", token);
        }
    }    
}

