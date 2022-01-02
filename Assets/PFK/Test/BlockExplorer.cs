using TMPro;
using UnityEngine;

namespace PFK.Test
{
    public class BlockExplorer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private int _testBlockNumner;
        
        private void Start()
        {
            FindObjectOfType<PFK.BlockExplorer>().OnBlockChange += UpdateText;
        }

        private void UpdateText(int blockNumber)
        {
            _text?.SetText($"Current block number is {blockNumber}");
        }

        [ContextMenu("Update text")]
        private void UpdateText()
        {
            UpdateText(_testBlockNumner);
        }
    }
}
