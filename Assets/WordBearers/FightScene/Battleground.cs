using UnityEngine;

namespace WordBearers.FightScene
{
    public class Battleground : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite[] _battlegrounds;
        
        private void OnValidate()
        {
            if (_battlegrounds.Length == 0) return;

            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = _battlegrounds[UnityEngine.Random.Range(0, _battlegrounds.Length)];
        }
    }
}
