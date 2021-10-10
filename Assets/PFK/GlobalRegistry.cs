using UnityEngine;

namespace PFK
{
    [CreateAssetMenu(menuName = "PFK/Global Registry")]
    public class GlobalRegistry : ScriptableObject
    {
        private static GlobalRegistry _instance;

        [SerializeField] private VFX.Controller _vfxController;

        public VFX.Controller VFXController => _vfxController;
        
        public static GlobalRegistry GetGlobalRegistry()
        {
            if (_instance == null)
                _instance = Resources.Load<GlobalRegistry>("GlobalRegistry");

            return _instance;
        }
    }
}
