using PFK;
using TMPro;
using UnityEngine;

namespace WordBearers.ProfileScene.Map
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private int _index;
        [SerializeField] private GameObject _currentLevelBadge;
        [SerializeField] private GameObject _padlock;

        private void OnValidate()
        {
            _text?.SetText((_index+1).ToString());
            _renderer.color = Color.white;
        }

        private void Start()
        {
            int level = PlayerState.GetInstance().State.level;
            _currentLevelBadge.SetActive(level == _index);
            _padlock.SetActive(level < _index);
        }
    }
}
