using UnityEngine;
using UnityEngine.Serialization;

namespace PFK.Shared
{
    public abstract class LevelDependencies : MonoBehaviour
    {
        [SerializeField] protected int _requiredLevel;
        [SerializeField] protected bool _lessThan;
        [SerializeField] protected bool _equals;
        [FormerlySerializedAs("_graterThan")] [SerializeField] protected bool _greaterThan;

        private bool Satisfied(int level)
        {
            return
                (_lessThan && level < _requiredLevel)
                || (_equals && level == _requiredLevel)
                || (_greaterThan && level > _requiredLevel);
        }

        private void OnEnable()
        {
            PlayerState.OnBaseStateChange += CheckLevelDependencies;
            CheckLevelDependencies(PlayerState.GetInstance());
        }

        private void OnDisable()
        {
            PlayerState.OnBaseStateChange -= CheckLevelDependencies;
        }

        private void CheckLevelDependencies(PlayerState state)
        {
            if (Satisfied(state.State.level))
            {
                WhenSatisfied();
            }
            else
            {
                WhenNotSatisfied();
            }
        }

        protected abstract void WhenSatisfied();
        protected abstract void WhenNotSatisfied();
    }
}
