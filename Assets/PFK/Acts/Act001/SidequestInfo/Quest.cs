using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class Quest : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        [SerializeField] private int _index;
        [SerializeField] private TMP_Text _countdown;

        private void OnValidate()
        {
            _index = transform.GetSiblingIndex();
        }

        private void Start()
        {
            FindObjectOfType<Manager>().Register(_index, this);
        }
        
        public void SetCooldown(double seconds)
        {
            jsPrintString($"Quest received the cooldown: {seconds}");
            
            if (seconds > 0)
            {
                _countdown.gameObject.SetActive(true);
                System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);

                List<string> timeParts = new();

                if (time.Days > 0) timeParts.Add($"{time.Days}d");
                if (time.Hours > 0) timeParts.Add($"{time.Hours}h");
                if (time.Minutes > 0) timeParts.Add($"{time.Minutes}m");
                if (time.Seconds > 0) timeParts.Add($"{time.Seconds}s");
                
                _countdown.SetText(string.Join(' ', timeParts));
            }
            else
            {
                _countdown.gameObject.SetActive(false);
            }
        }

        
    }
}
