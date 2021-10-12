using UnityEngine;

namespace PFK.Test
{
    public class ColorToWhite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        
        void Start()
        {
            _renderer.color = Color.white;
        }
        
    }
}
