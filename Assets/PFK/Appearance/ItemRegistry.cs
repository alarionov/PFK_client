using System.Linq;
using UnityEngine;

namespace PFK.Appearance
{
    [CreateAssetMenu(menuName = "PFK/Item Registry")]
    public class ItemRegistry : ScriptableObject
    {
        private static ItemRegistry _instance;
        
        [SerializeField] private WeaponSet[] _weaponSets;
        [SerializeField] private ArmorSet[] _armorSets;

        public static ItemRegistry GetInstance()
        {
            if (_instance is null)
                _instance = Resources.Load<ItemRegistry>("ItemRegistry");
            
            return _instance;
        }

        public static WeaponSet GetWeaponSet(WeaponType weaponType)
        {
            return GetInstance()._weaponSets.First(x => x.WeaponType == weaponType);
        }


        public static ArmorSet GetArmorSet(ArmorType armorType)
        {
            return GetInstance()._armorSets.First(x => x.ArmorType == armorType);
        }
    }
}
