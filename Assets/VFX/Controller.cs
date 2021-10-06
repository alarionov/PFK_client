using TMPro;
using UnityEngine;

namespace VFX
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Animator _bloodSplashAnimator;
        private static readonly int Splash = Animator.StringToHash("Splash");

        [SerializeField] private Animator _damageValueAnimator;
        private static readonly int Show = Animator.StringToHash("Show");

        public void ShowDamage(int i)
        {
            _damageValueAnimator.GetComponentInChildren<TMP_Text>(true)?.SetText(i.ToString());
            
            _bloodSplashAnimator.SetTrigger(Splash);
            _damageValueAnimator.SetTrigger(Show);
        }
        
    }
}
