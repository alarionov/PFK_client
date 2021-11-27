using UnityEngine;

namespace PFK.Test
{
    public class TestState : MonoBehaviour
    {
        [SerializeField] private int _level;

        [ContextMenu("Set Level")]
        private void SetLevel()
        {
            Debug.LogWarning("Nothing happend");
        }
    }
}
