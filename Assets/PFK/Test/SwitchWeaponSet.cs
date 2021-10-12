using PFK.Appearance;
using UnityEngine;

namespace PFK.Test
{
    public class SwitchWeaponSet : MonoBehaviour
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private PFK.PlayerCharacter.Character _character;

        public void Switch()
        {
            Debug.Log(_weaponType);
            _character.Wear(ItemRegistry.GetWeaponSet(_weaponType));
        }
    }
}
