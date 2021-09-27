using Characters.Controllers;
using TMPro;
using UnityEngine;

namespace PFK
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private BaseStats _stats;

        public BaseStats BaseStats => _stats;
        
        private CharacterAnimationController _animationController; 
        
        public enum UpdateMode
        {
            Set,
            Add,
            Sub
        }

        [Header("Stats")]
        [SerializeField] private TMP_Text _attack;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _armour;

        private void Awake()
        {
            Render();
        }

        private void Start()
        {
            _animationController = GetComponent<CharacterAnimationController>();
        }

        public void UpdateStats(BaseStats stats, UpdateMode mode = UpdateMode.Set)
        {
            gameObject.SetActive(true);

            switch (mode)
            {
                case UpdateMode.Add:
                    _stats.Attack += stats.Attack;
                    _stats.Health += stats.Health;
                    _stats.Armour += stats.Armour;
                    break;
                case UpdateMode.Sub:
                    _stats.Attack = Mathf.Max(0, _stats.Attack - stats.Attack); 
                    _stats.Health = Mathf.Max(0, _stats.Health - stats.Health);
                    _stats.Armour = Mathf.Max(0, _stats.Armour - stats.Armour);
                    break;
                case UpdateMode.Set:
                    _stats = new BaseStats()
                    {
                        Attack = stats.Attack,
                        Health = stats.Health,
                        Armour = stats.Armour
                    };
                    break;
                default:
                    throw new System.Exception("Invalid mode");
            }
            
            Render();
        }

        public void ShowAttack() => _animationController.Slashing();

        public void ShowDamage(FightAction action)
        {
            UpdateStats(new BaseStats(){ Health = action.Damage }, UpdateMode.Sub);
            
            if (action.Type == AttackType.Miss || action.Damage == 0 )
            {
                _animationController.Blinking();
            }
            else if (_stats.Health <= 0)
            {
                _animationController.Dying();
            }
            else
            {
                _animationController.Hurt();
            }

            if (action.Type == AttackType.Miss)
            {
                //_missPanel.SetActive(true);
            }
            else
            {
                //_damagePanel.SetActive(true);
                //_critIndicator.SetActive(action.Type == AttackType.Crit);
                //_damageValue.SetText(action.Damage.ToString());
            }
        }
        
        public void ShowDamage(int damage)
        {
            UpdateStats(new BaseStats(){ Health = damage }, UpdateMode.Sub);
            
            if (damage == 0)
            {
                _animationController.Blinking();
            }
            else if (_stats.Health <= 0)
            {
                _animationController.Dying();
            }
            else
            {
                _animationController.Hurt();
            }
            
            //_damagePanel.SetActive(true);
            //_damageValue.SetText(damage.ToString());
        }

        public void ShowVampirism()
        {
            UpdateStats(new BaseStats(){Health = 1}, UpdateMode.Add);
            
            //_vampPanel.SetActive(true);
            //_healValue.SetText("1");
        }

        public void ShowReflect()
        {
            //_reflectPanel.SetActive(true);
        }

        private void Render()
        {
            _attack.SetText(_stats.Attack.ToString());
            _health.SetText(_stats.Health.ToString());
            _armour.SetText(_stats.Armour.ToString());
        }
    }
}
