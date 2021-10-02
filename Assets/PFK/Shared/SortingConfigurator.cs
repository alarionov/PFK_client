using UnityEngine;

namespace PFK.Shared
{
    public class SortingConfigurator : MonoBehaviour
    {
        [SerializeField] private string _sortingLayer;
        [SerializeField] private int _baseSortingOrder;

        [ContextMenu("Set Sorting Options")]
        private void SetSortingOptions()
        {
            foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.sortingLayerName = _sortingLayer;
                renderer.sortingOrder += _baseSortingOrder;
            }
        }
    }
}
