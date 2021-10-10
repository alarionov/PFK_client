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
        
        [ContextMenu("Show Crit Damage")]
        public void ShowCritDamage()
        {
            _animationController.Hurt();
            _vfxController.ShowDamage(1, crit:true);
        }
        
        [ContextMenu("Show Miss Damage")]
        public void ShowMissDamage()
        {
            _animationController.Blinking();
            _vfxController.ShowDamage(1, miss:true);
        }
        
        [ContextMenu("Show Blocked Damage")]
        public void ShowBlockedDamage()
        {
            _animationController.Hurt();
            _vfxController.ShowDamage(1, blocked:true);
        }
        
        [ContextMenu("Show Blocked Damage2")]
        public void ShowBlockedDamage2()
        {
            _animationController.Blinking();
            _vfxController.ShowDamage(1, blocked:true);
        }
    }
}
