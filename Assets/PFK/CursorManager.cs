using UnityEngine;

namespace PFK
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D _defaultCursor;
        [SerializeField] private Texture2D _handCursor;
        [SerializeField] private Texture2D _magnifierCursor;
        
        private static CursorManager _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }

            _instance = this;

            DontDestroyOnLoad(this);
        }

        public static void SetCursor(CursorType cursor)
        {
            Texture2D currentCursor;
            
            switch (cursor)
            {
                case CursorType.Hand:
                    currentCursor = _instance._handCursor;
                    break;
                case CursorType.Magnifier:
                    currentCursor = _instance._magnifierCursor;
                    break;
                default:
                    currentCursor = _instance._defaultCursor;
                    break;
            }
            
            Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        public static void SetDefaultCursor()
        {
            SetCursor(CursorType.Default);
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}
