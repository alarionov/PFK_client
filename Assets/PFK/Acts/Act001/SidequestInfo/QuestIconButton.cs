using PFK.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class QuestIconButton : QuestElement
    {
        [SerializeField] private Image _flag;
        [SerializeField] private Button _button;
        [SerializeField] private LevelDependency _dependency;

        private Clickable _clickable;

        private void Start()
        {
            _clickable = _button.gameObject.GetComponent<Clickable>();
        }

        private void Update()
        {
            bool active = _cooldown <= 0 && !_dependency.Locked;
            _flag.gameObject.SetActive(active);
            _button.enabled = active;
            _clickable.enabled = active;
        }

        private void OnValidate()
        {
            _index = transform.GetSiblingIndex() - 1;
        }
    }
}
