using UnityEngine;

namespace WordBearers.Shared
{
    public class HideCursor : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            Cursor.visible = true;
        }
    }
}
