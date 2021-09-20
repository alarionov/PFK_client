using Characters.Controllers;
using UnityEngine;

namespace PFK
{
    public class FightScene : MonoBehaviour
    {
        [SerializeField] private CharacterAnimationController _player;
        [SerializeField] private CharacterAnimationController[] _enemies;
    }
}
