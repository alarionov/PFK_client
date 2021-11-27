using System.Runtime.InteropServices;
using UnityEngine;

namespace PFK.GameMenu.CharacterSelect
{
    public class ChooseButton : MonoBehaviour
    {
        
        [DllImport("__Internal")]
        private static extern void jsGetCharacter(int token);
        
        public void Choose()
        {
        }

        public void Choose(int token)
        {
            jsGetCharacter(token);
        }
    }    
}

