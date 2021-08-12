using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WordBearers.Shared.Buffs
{
    public class Buff : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;
        [SerializeField] private string _buffName;
        [SerializeField] private TMP_Text _buffText;

        private void OnValidate()
        {
            _buffText?.SetText(_buffName);
            _renderer.color = _inactiveColor;
        }

        public void SetText(string value)
        {
            _buffName = value;
            _buffText?.SetText(value);
        }

        public void SetSprite(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }

        public void SetState(bool state)
        {
            _renderer.color = state ? _activeColor : _inactiveColor;
        }
    }
}
