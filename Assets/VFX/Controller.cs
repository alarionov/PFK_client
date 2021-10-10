using TMPro;
using UnityEngine;

namespace VFX
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float _hitTextSize;
        [SerializeField] private Color _hitTextColor;
        
        [SerializeField] private float _critTextSize;
        [SerializeField] private Color _critTextColor;
        
        [SerializeField] private Animator _bloodSplashAnimator;
        private static readonly int Splash = Animator.StringToHash("Splash");

        [SerializeField] private Animator _damageValueAnimator;
        private static readonly int Show = Animator.StringToHash("Show");

        public void ShowDamage(int damage, bool crit = false, bool blocked = false, bool miss = false)
        {
            TMP_Text text = _damageValueAnimator.GetComponentInChildren<TMP_Text>(true);
            
            if (miss)
            {
                text.SetText("miss");
            }
            else if (blocked)
            {
                text.SetText("blocked");
            }
            else
            {
                text.SetText(damage.ToString());    
                _bloodSplashAnimator.SetTrigger(Splash);
            }
            
            if (crit)
            {
                text.fontSize = _critTextSize;
                text.color = _critTextColor;
            }
            else
            {
                text.fontSize = _hitTextSize;
                text.color = _hitTextColor;
            }
            
            _damageValueAnimator.SetTrigger(Show);
        }
        
    }
}
