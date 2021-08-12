using UnityEngine;

namespace Characters.Controllers
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [ContextMenu("Idle")]
        public void Idle() => _animator.SetTrigger("Idle");
        
        [ContextMenu("Dying")]
        public void Dying() => _animator.SetTrigger("Dying");
        
        [ContextMenu("Hurt")]
        public void Hurt() => _animator.SetTrigger("Hurt");
        
        [ContextMenu("Slashing")]
        public void Slashing() => _animator.SetTrigger("Slashing");
        
        [ContextMenu("Blinking")]
        public void Blinking() => _animator.SetTrigger("Blinking");
    }
}
