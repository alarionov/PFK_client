using PFK.Shared;
using UnityEngine;

namespace PFK.Acts
{
    public class LevelDependantButton : LevelDependencies
    {
        [SerializeField] private MonoBehaviour[] _components;
        
        protected override void WhenSatisfied()
        {
            foreach (MonoBehaviour _ in _components)
            {
                _.enabled = true;
            }
        }

        protected override void WhenNotSatisfied()
        {
            foreach (MonoBehaviour _ in _components)
            {
                _.enabled = false;
            }
        }
    }
}
