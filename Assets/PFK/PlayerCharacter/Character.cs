using UnityEngine;

namespace PFK.PlayerCharacter
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _leftLeg;
        [SerializeField] private SpriteRenderer _rightLeg;
        
        [SerializeField] private SpriteRenderer _head;
        [SerializeField] private SpriteRenderer _face;
        
        [SerializeField] private SpriteRenderer _leftArm;
        [SerializeField] private SpriteRenderer _leftHand;
        [SerializeField] private SpriteRenderer _rightArm;
        [SerializeField] private SpriteRenderer _rightHand;
        
        [SerializeField] private SpriteRenderer _weapon;
        [SerializeField] private SpriteRenderer _slashFX;
        
        [SerializeField] private SpriteRenderer _body;

        [SerializeField] private Sprite _tmpHeadSprite;

        private void Awake()
        {
            _face.gameObject.SetActive(false);
            _head.sprite = _tmpHeadSprite;
        }
    }

}
