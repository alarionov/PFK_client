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

        [DllImport("__Internal")]
        private static extern void jsGetAct1SidequestCooldowns(string address, int token);
        
        public void Choose(int token)
        {
            string purrContract = "0x4e0bA77E4Bb93ACa2B5D219A5CE896F144F81f55";
            jsGetCharacter(purrContract, token);
            jsGetAct1Progress(purrContract, token);
            jsGetAct1SidequestCooldowns(purrContract, token);
        }
    }    
}

