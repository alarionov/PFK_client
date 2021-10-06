using Characters.Controllers;
using UnityEngine;

namespace PFK.Test
{
    public class TestVFX : MonoBehaviour
    {
        [SerializeField] private CharacterAnimationController _animationController;
        [SerializeField] private VFX.Controller _vfxController;

        [ContextMenu("Show Damage")]
        public void ShowDamage()
        {
            _animationController.Hurt();
            _vfxController.ShowDamage(1);
        }
    }
}
