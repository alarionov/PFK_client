using System;
using UnityEngine;

namespace WordBearers
{
    [CreateAssetMenu(menuName = "WordBearers/SpellRegistry")]
    public class SpellRegistry : ScriptableObject
    {
        [System.Serializable]
        public class Spell
        {
            public int Index;
            public string Name;
            public int WordCount;
            public Sprite Icon;
        }
        
        [SerializeField] private Spell[] _spells;

        public Spell[] Spells => _spells;

        private void OnValidate()
        {
            for (int i = 0; i < _spells.Length; ++i)
            {
                _spells[i].Index = i;
            }
        }
    }
}
