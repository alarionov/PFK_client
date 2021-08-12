using UnityEngine;

namespace WordBearers.Shared
{
    public class Clickable : MonoBehaviour, IClickable
    {
        [SerializeField] private CursorType _cursor;

        public CursorType GetCursor() => _cursor;
    }
}
