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
        }
    }

}
