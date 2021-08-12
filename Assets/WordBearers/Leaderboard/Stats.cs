using UnityEngine;

namespace WordBearers.Leaderboard
{
    [System.Serializable]
    public class Stats
    {
        public static System.Action<Stats> OnLeaderboardChange;
        private static Stats _instance;
        
        
        public Record[] Records;

        public static Stats GetStats()
        {
            if (_instance is null)
            {
                _instance = new Stats();
            }

            return _instance;
        }

        public static void LoadStats(string encoded)
        {
            _instance = JsonUtility.FromJson<Stats>(encoded);
            OnLeaderboardChange?.Invoke(_instance);
        }
    }
}
