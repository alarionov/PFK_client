using PFK.Appearance;
using UnityEngine;

namespace PFK.PlayerCharacter
{
    public class Character : MonoBehaviour
    {
        [Header("Head Elements")] 
        [SerializeField] private SpriteRenderer _head;
        [SerializeField] private SpriteRenderer _face;
        
        [Header("Armor Set Elements")]
        [SerializeField] private SpriteRenderer _leftLeg;
        [SerializeField] private SpriteRenderer _rightLeg;
        [SerializeField] private SpriteRenderer _leftArm;
        [SerializeField] private SpriteRenderer _leftHand;
        [SerializeField] private SpriteRenderer _rightArm;
        [SerializeField] private SpriteRenderer _rightHand;
        [SerializeField] private SpriteRenderer _body;

        [Header("Weapon Set Elements")]
        [SerializeField] private SpriteRenderer _weapon;
        [SerializeField] private SpriteRenderer _slashFX;
        
        private void Awake()
        {
            _face.gameObject.SetActive(false);
            
            int tokenId = 3029;
            _head.sprite = Resources.Load<Sprite>($"PurrHeads/{tokenId}");

            Wear(ItemRegistry.GetWeaponSet(WeaponType.WoodenStick));
            Wear(ItemRegistry.GetArmorSet(ArmorType.PrisonRobe));
        }

        public void Wear(WeaponSet weaponSet)
        {
            _weapon.sprite = weaponSet.Weapon;
            _slashFX.sprite = weaponSet.SlashFX;
        }

        public void Wear(ArmorSet armorSet)
        {
            _leftLeg.sprite = armorSet.LeftLeg;
            _rightLeg.sprite = armorSet.RightLeg;
            _leftArm.sprite = armorSet.LeftArm;
            _leftHand.sprite = armorSet.LeftHand;
            _rightArm.sprite = armorSet.RightArm;
            _rightHand.sprite = armorSet.RightHand;
            _body.sprite = armorSet.Body;
        }
    }

}
