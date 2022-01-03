using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class QuestCooldown : QuestElement
    {
        [SerializeField] private TMP_Text _timer;

        private void OnValidate()
        {
            _index = transform.GetSiblingIndex();
        }
        
        private void Update()
        {
            if (_cooldown > 0)
            {
                _timer.gameObject.SetActive(true);
                System.TimeSpan time = System.TimeSpan.FromSeconds(_cooldown);

                List<string> timeParts = new();

                if (time.Days > 0) timeParts.Add($"{time.Days}d");
                if (time.Hours > 0) timeParts.Add($"{time.Hours}h");
                if (time.Minutes > 0) timeParts.Add($"{time.Minutes}m");
                if (time.Seconds > 0) timeParts.Add($"{time.Seconds}s");
                
                _timer.SetText(string.Join(' ', timeParts));
            }
            else
            {
                _timer.gameObject.SetActive(false);
            }
        }
    }
}
