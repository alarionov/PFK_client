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

        public void Choose(int token)
        {
            jsGetCharacter("0x14532E037b0e63A8882C6887351AB135CAAE4D0D", token);
            jsGetAct1Progress("0x14532E037b0e63A8882C6887351AB135CAAE4D0D", token);
        }
    }    
}

