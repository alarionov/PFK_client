using PFK.Shared;
using UnityEngine;

namespace PFK.Acts
{
    public class LevelCheckingParent : LevelDependencies
    {
        [SerializeField] private GameObject[] _children;

        protected override void WhenSatisfied()
        {
            foreach (GameObject _ in _children)
            {
                _.SetActive(true);
            }
        }

        protected override void WhenNotSatisfied()
        {
            foreach (GameObject _ in _children)
            {
                _.SetActive(false);
            }
        }
    }
}
