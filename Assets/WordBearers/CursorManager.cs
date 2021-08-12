using UnityEngine;

namespace WordBearers
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D _defaultCursor;
        [SerializeField] private Texture2D _hoveringCursor;
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

        private void Update()
        {
            IClickable clickable;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);

            if (collider != null)
            {
                clickable = collider.gameObject.GetComponent<IClickable>();

                if (clickable != null)
                {
                    SetCursor(clickable.GetCursor());
                    return;
                }
            }
            
            SetCursor(CursorType.Default);
        }

        private void SetCursor(CursorType cursor)
        {
            Texture2D currentCursor;
            
            switch (cursor)
            {
                case CursorType.Hovering:
                    currentCursor = _hoveringCursor;
                    break;
                case CursorType.Magnifier:
                    currentCursor = _magnifierCursor;
                    break;
                default:
                    currentCursor = _defaultCursor;
                    break;
            }
            
            Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}
