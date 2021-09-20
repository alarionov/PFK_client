using PFK;
using UnityEngine;

namespace WordBearers.Shared
{
    public class LoadProfile : MonoBehaviour
    {
        [SerializeField] private float _waitingDuration;

        private float _timer;
        private WalletManager _walletManager;
        
        private void Start()
        {
            _timer = _waitingDuration;
            _walletManager = FindObjectOfType<WalletManager>();
        }

        private void FixedUpdate()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _walletManager.SetState(Fight.Instance.FightParams.NewState);
            }
        }
    }
}
