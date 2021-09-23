using UnityEngine;
using UnityEngine.EventSystems;

namespace PFK.Shared
{
    public class Clickable : MonoBehaviour, IClickable,  IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CursorType _cursor;
        [SerializeField] private GameObject _outline; 

        public CursorType GetCursor() => _cursor;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorManager.SetCursor(_cursor);
            
            if (_outline != null) 
                _outline.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorManager.SetDefaultCursor();
            
            if (_outline != null) 
                _outline.SetActive(false);
        }
    }
}
