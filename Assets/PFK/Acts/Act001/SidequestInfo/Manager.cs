using System.Collections.Generic;
using UnityEngine;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class Manager : MonoBehaviour
    {
        private class Quest
        {
            public int Index;
            public QuestElement Element;
        }

        [SerializeField] private BlockExplorer _explorer;
        
        private readonly List<Quest> _questElements = new ();
        private readonly int[] _cooldowns = new int[6];

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

        private void OnBlockChange(int blockNumber)
        {
            foreach (Quest quest in _questElements)
            {
                UpdateQuestCooldown(quest.Element, _cooldowns[quest.Index]);
            }
        }
        
        public void Register(int index, QuestElement element)
        {
            _questElements.Add(new () {Index = index, Element = element});
            
            UpdateQuestCooldown(element, _cooldowns[index]);
        }

        public void Deregister(QuestElement element)
        {
            _questElements.RemoveAll(quest => quest.Element == element);
        }

        private void UpdateQuestCooldown(QuestElement element, int activeAfter)
        {
            int cooldown = (activeAfter - _explorer.CurrentBlock) * _explorer.SecondsPerBlock;
            element.SetCooldown(cooldown);
        }

        public void SetQuestCooldown(string json)
        {
            Params cooldownParams = JsonUtility.FromJson<Params>(json);

            _cooldowns[cooldownParams.index] = cooldownParams.blockNumber;

            foreach (Quest quest in _questElements)
            {
                if (quest.Index != cooldownParams.index) continue;
                
                UpdateQuestCooldown(quest.Element, _cooldowns[quest.Index]);    
            }
        }
    }
}
