using TMPro;
using UnityEngine;

namespace PFK.Shared.DetailedInfo.Character
{
    public class Attribute : MonoBehaviour
    {
        [SerializeField] private BaseStats.AttributeType _type;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _value;

        private void OnValidate()
        {
            _name.SetText(_type.ToString());
        }
    }
}
