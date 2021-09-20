using PFK;
using TMPro;
using UnityEngine;

namespace WordBearers.ProfileScene
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private TMP_Text _exp;
        [SerializeField] private TMP_Text _level;

        [Header("Stats")]
        [SerializeField] private TMP_Text _attack;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _armour;

        [Header("Buffed Stats")] 
        [SerializeField] private Color _buffedStatsColor;
        [SerializeField] private GameObject _buffedStatsPlate;
        [SerializeField] private TMP_Text _buffedAttack;
        [SerializeField] private TMP_Text _buffedHealth;
        [SerializeField] private TMP_Text _buffedArmour;
        
        private void Start()
        {
            Render(PlayerState.GetInstance());
            PlayerState.OnCharacterChange += Render;
            PlayerState.OnBuffsChange += Render;
        }

        private void OnDestroy()
        {
            PlayerState.OnCharacterChange -= Render;
            PlayerState.OnBuffsChange -= Render;
        }

        private void Render(PlayerState state)
        {
            WordBearers.Character character = state.Character;

            _exp?.SetText(character.Exp.ToString());
            _level?.SetText(character.Level.ToString());

            BaseStats stats = character.Stats;
            BaseStats buffedStats = BaseStats.ApplyBuffs(stats, state.Buffs, true);
            
            _attack?.SetText(stats.Attack.ToString());
            _health?.SetText(stats.Health.ToString());
            _armour?.SetText(stats.Armour.ToString());

            if (stats.Equals(buffedStats))
            {
                _buffedStatsPlate.SetActive(false);
            }
            else
            {
                _buffedStatsPlate.SetActive(true);
                
                _buffedAttack.SetText(buffedStats.Attack.ToString());
                if (stats.Attack != buffedStats.Attack)
                {
                    _buffedAttack.color = _buffedStatsColor;
                }
                
                _buffedHealth.SetText(buffedStats.Health.ToString());
                if (stats.Health != buffedStats.Health)
                {
                    _buffedHealth.color = _buffedStatsColor;
                }
                
                _buffedArmour.SetText(buffedStats.Armour.ToString());
                if (stats.Armour != buffedStats.Armour)
                {
                    _buffedArmour.color = _buffedStatsColor;
                }
            }
        }
    }
}
