using UnityEngine;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class QuestElement : MonoBehaviour
    {
        [SerializeField] protected int _index;
        
        protected int _cooldown;
        
        private Manager _manager;

        protected virtual void Awake()
        {
            _manager = FindObjectOfType<Manager>();
            _manager.Register(_index, this);
        }
        
        protected virtual void OnDestroy()
        {
            _manager.Deregister(this);
        }

        public void SetCooldown(int seconds)
        {
            _cooldown = seconds;
        }
    }
}