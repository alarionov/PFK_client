using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WordBearers.Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private Slot[] _slots;
        
        [DllImport("__Internal")]
        private static extern void jsGetLeaderboard();

        private void Start()
        {
            Stats.OnLeaderboardChange += UpdateLeaderboard;
            
            GetLeaderboard();
        }

        private void OnDestroy()
        {
            Stats.OnLeaderboardChange -= UpdateLeaderboard;
        }

        public void GetLeaderboard() => jsGetLeaderboard();

        private void UpdateLeaderboard(Stats stats)
        {
            for (int i = 0; i < stats.Records.Length; ++i)
            {
                _slots[i].SetValues(stats.Records[i]);
            }
        }
    }
}
