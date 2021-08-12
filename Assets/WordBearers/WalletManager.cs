using UnityEngine;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using WordBearers.Leaderboard;

namespace WordBearers
{
    public class WalletManager : MonoBehaviour
    {
        private static WalletManager _instance;
        
        [DllImport("__Internal")]
        private static extern void jsConnectWallet();

        [DllImport("__Internal")]
        private static extern void jsGetState(string address);
        
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);

        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private GameObject _noWalletMessage;

        [SerializeField] private string _profileScene;
        [SerializeField] private string _registerScene;
        [SerializeField] private string _fightScene;

        private PlayerState _state;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            HideNoWalletError();
            HideLoadingScreen();
            _state = PlayerState.GetInstance();
        }

        public void ConnectWallet() => jsConnectWallet();

        private static bool ValidateWallet(string wallet)
        {
            Regex pattern = new Regex("^0x[a-zA-Z0-9]{40}$");

            return pattern.IsMatch(wallet);
        }

        /* Called from html */

        public void ShowNoWalletError() => _noWalletMessage.SetActive(true);


        public void HideNoWalletError() => _noWalletMessage.SetActive(false);
        
        public void ShowLoadingScreen() => _loadingScreen.SetActive(true);

        public void HideLoadingScreen() => _loadingScreen.SetActive(false);

        public void SetWallet(string wallet)
        {
            if (!ValidateWallet(wallet))
            {
                return;
            }
            
            _state.Wallet = wallet;

            jsGetState(_state.Wallet);
        }

        public void SetState(string encodedState)
        {
            UpdateState(encodedState);
            SceneManager.LoadScene(_state.State.tokenId > 0 ? _profileScene : _registerScene, LoadSceneMode.Single);
        }

        public void SetState(BaseState state)
        {
            UpdateState(state);
            SceneManager.LoadScene(_state.State.tokenId > 0 ? _profileScene : _registerScene, LoadSceneMode.Single);
        }

        public void UpdateState(string encodedState)
        {
            _state.LoadState(encodedState);
        }

        public void UpdateState(BaseState state)
        {
            _state.LoadState(state);
        }

        public void BuffsLoaded(string encodedBuffs)
        {
            _state.LoadBuffs(encodedBuffs);
        }

        public void FightLoaded(string encodedFight)
        {
            Fight.LoadParams(encodedFight);
            jsPrintString("Loading Fight!");
            SceneManager.LoadScene(_fightScene, LoadSceneMode.Single);
        }
        
        public void CharacterLoaded(string encoded)
        {
            _state.LoadCharacter(encoded);
        }

        public void NewStatsLoaded(string encoded)
        {
            _state.UpdateStats(encoded);
        }

        public void LeaderboardLoaded(string encoded)
        {
            Stats.LoadStats(encoded);
        }
    }
}
