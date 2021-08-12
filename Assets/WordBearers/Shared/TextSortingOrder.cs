using TMPro;
using UnityEngine;

namespace WordBearers.Shared
{
    public class TextSortingOrder : MonoBehaviour
    {
        [SerializeField] private string _sortingLayer;
        [SerializeField] private int _sortingOrder;

        private void Start()
        {
            TMP_Text text = GetComponent<TMP_Text>();
            
            if (!string.IsNullOrEmpty(_sortingLayer))
                text.canvas.sortingLayerName = _sortingLayer;

            text.canvas.sortingOrder = _sortingOrder;
        }
    }
}
