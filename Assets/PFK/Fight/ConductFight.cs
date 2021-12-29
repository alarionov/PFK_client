using System.Runtime.InteropServices;
using UnityEngine;

namespace PFK.Fight
{
    public class ConductFight : MonoBehaviour
    {
        [SerializeField] protected string _contractAddress;
        [SerializeField] protected int _level;

        [DllImport("__Internal")]
        private static extern void jsConductFight(string wallet, string mapAddress, int level, string characterContract, int token);

        public void Conduct()
        {
            PlayerState state = PlayerState.GetInstance();
            jsConductFight(
                state.Wallet,
                _contractAddress, 
                _level,
                state.Character.ContractAddress, 
                state.Character.TokenId);
        }
    }
}