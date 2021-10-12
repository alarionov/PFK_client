using PFK.Appearance;
using UnityEngine;

namespace PFK.Test
{
    public class SwitchArmorSet : MonoBehaviour
    {
        [SerializeField] private ArmorType _armorType;
        [SerializeField] private PFK.PlayerCharacter.Character _character;

        public void Switch()
        {
            Debug.Log(_armorType);
            _character.Wear(ItemRegistry.GetArmorSet(_armorType));
        }
    }
}
