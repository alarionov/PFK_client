using PFK.Acts.SceneRegistry;
using UnityEngine;

namespace PFK.Fight
{
    public class ConductFightRegistry : ConductFight
    {
        [SerializeField] private SceneRegistry _registry;
        [SerializeField] private string _entryTitle;

        private void OnValidate()
        {
            SetContractAddress();
        }

        private void Start()
        {
            SetContractAddress();
        }

        private void SetContractAddress()
        {
            if (_registry == null) return;
            if (string.IsNullOrEmpty(_entryTitle)) return;

            _contractAddress = _registry.GetEntryByTitle(_entryTitle).ContractAddress;
        }
    }
}
