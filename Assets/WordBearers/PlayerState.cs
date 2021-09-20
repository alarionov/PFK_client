using PFK;
using UnityEngine;

namespace WordBearers
{
    public class PlayerState
    {
        public static event System.Action<PlayerState> OnChange;
        public static event System.Action<PlayerState> OnBaseStateChange;
        public static event System.Action<PlayerState> OnBuffsChange;
        
        public static event System.Action<PlayerState> OnCharacterChange;

        private static PlayerState _instance;

        public string Wallet { get; set; }

        public BaseState State { get; private set; }
        public bool[] Buffs { get; private set; }

        public Character Character { get; private set; }

        public static PlayerState GetInstance()
        {
            if (_instance is null)
            {
                _instance = new PlayerState();
            }

            return _instance;
        }

        public void LoadState(string encodedState)
        {
            LoadState(JsonUtility.FromJson<BaseState>(encodedState));
        }
        
        public void LoadState(BaseState state)
        {
            State = state;
            OnChange?.Invoke(this);
            OnBaseStateChange?.Invoke(this);
        }

        public void LoadBuffs(string encodedBuffs)
        {
            Buffs = JsonUtility.FromJson<BuffWrapper>(encodedBuffs).Buffs;
            OnChange?.Invoke(this);
            OnBuffsChange?.Invoke(this);
        }

        public void LoadCharacter(string encodedData)
        {
            Character = JsonUtility.FromJson<Character>(encodedData);
            OnChange?.Invoke(this);
            OnCharacterChange?.Invoke(this);
        }

        public void UpdateStats(string encoded)
        {
            Character character = JsonUtility.FromJson<Character>(encoded);
            Character.Stats = character.Stats;
            Character.Upgrades = character.Upgrades;
            OnChange?.Invoke(this);
            OnCharacterChange?.Invoke(this);
        }
    }
}
