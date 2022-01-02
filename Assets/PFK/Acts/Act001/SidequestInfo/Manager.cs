using System.Runtime.InteropServices;
using UnityEngine;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class Manager : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        [SerializeField] private BlockExplorer _explorer;
        
        private readonly Quest[] _quests = new Quest[6];
        private readonly int[] _activeAfter = new int[6];

        [System.Serializable]
        public class Params
        {
            public int index;
            public int blockNumber;
        }

        private void Start()
        {
            _explorer.OnBlockChange += OnBlockChange;
        }

        private void OnBlockChange(int blockNumnber)
        {
            for (int i = 0; i < _quests.Length; ++i)
            {
                if (_quests[i] is null) continue;
                
                UpdateQuestCooldown(_quests[i], _activeAfter[i]);
            }
        }
        
        public void Register(int index, Quest details)
        {
            _quests[index] = details;
            
            UpdateQuestCooldown(details, _activeAfter[index]);
        }

        private void UpdateQuestCooldown(Quest quest, int activeAfter)
        {
            double cooldown = (double)(activeAfter - _explorer.CurrentBlock) * _explorer.SecondsPerBlock;
            jsPrintString($"{activeAfter - _explorer.CurrentBlock} {_explorer.SecondsPerBlock} {cooldown}");
            quest.SetCooldown(cooldown);
        }

        public void SetQuestCooldown(string json)
        {
            Params cooldownParams = JsonUtility.FromJson<Params>(json);

            _activeAfter[cooldownParams.index] = cooldownParams.blockNumber;

            if (_quests[cooldownParams.index] is null) return;
            
            UpdateQuestCooldown(_quests[cooldownParams.index], cooldownParams.blockNumber);
        }
    }
}
