using System.Collections;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using PFK.Acts.SceneRegistry;
using UnityEngine;
using UnityEngine.SceneManagement;
using WordBearers;
using WordBearers.Leaderboard;

namespace PFK
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
        
        [SerializeField] private SceneRegistry _fightScenes;
        
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

        public static WalletManager GetInstance()
        {
            return _instance;
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

        public void FightScene(FightWrapper fight)
        {
            StartCoroutine(LoadAndFight(fight));
        }

        private IEnumerator LoadAndFight(FightWrapper fight)
        {
            Fight.NewFight(fight.FightParams);
            
            ShowLoadingScreen();
            yield return new WaitForSeconds(1);
            
            AsyncOperation asyncLoad = 
                SceneManager.LoadSceneAsync(
                    _fightScenes.GetScene(fight.ContractAddress, fight.SceneIndex), 
                    LoadSceneMode.Single);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            HideLoadingScreen();
        }
    }
}
