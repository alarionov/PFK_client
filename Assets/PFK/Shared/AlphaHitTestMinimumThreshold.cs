using UnityEngine;
using UnityEngine.UI;

namespace PFK.Shared
{
    public class AlphaHitTestMinimumThreshold : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}
