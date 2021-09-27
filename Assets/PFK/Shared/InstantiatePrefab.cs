using UnityEngine;

namespace PFK.Shared
{
    public class InstantiatePrefab : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;
        
        public void Instantiate()
        {
            Instantiate(_prefab, _parent);
        }
    }
}
