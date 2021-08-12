using UnityEngine;

namespace Characters.Controllers
{
    public class FaceController : MonoBehaviour
    {
        [SerializeField] private Sprite[] _faces;
        [SerializeField] private SpriteRenderer _renderer;
        
        private void SetFace(int index)
        {
            _renderer.sprite = _faces[index];
        }
    }
}
