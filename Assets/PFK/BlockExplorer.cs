using System.Runtime.InteropServices;
using UnityEngine;

namespace PFK
{
    public class BlockExplorer : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        public event System.Action<int> OnBlockChange;

        [SerializeField] private int _secondsPerBlock = 1;
        private int _currentBlock;

        public int CurrentBlock => _currentBlock;
        public int SecondsPerBlock => _secondsPerBlock;

        public void SetCurrentBlock(int blockNumber)
        {
            _currentBlock = blockNumber;
            OnBlockChange?.Invoke(_currentBlock);
        }
    }
}
