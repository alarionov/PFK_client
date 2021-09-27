using UnityEngine;

namespace PFK.Shared
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        public void Destroy()
        {
            Destroy(_target);
        }
    }
}
