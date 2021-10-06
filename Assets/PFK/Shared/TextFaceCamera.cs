using UnityEngine;

namespace PFK.Shared
{
    public class TextFaceCamera : MonoBehaviour
    {
        [ContextMenu("Adjust Scale")]
        private void OnEnable()
        {
            if (transform.lossyScale.x < 0)
            {
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
            }
        }
    }
}
