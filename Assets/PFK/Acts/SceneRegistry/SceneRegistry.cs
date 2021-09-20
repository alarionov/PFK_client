using UnityEngine;

namespace PFK.Acts.SceneRegistry
{
    [CreateAssetMenu(menuName = "PFK/Scene Registry")]
    public class SceneRegistry : ScriptableObject
    {
        [SerializeField] private Entry[] _entries;

        public string GetScene(string contractAddress, int index)
        {
            foreach (Entry entry in _entries)
            {
                if (entry.ContractAddress == contractAddress)
                {
                    return entry.GetScene(index);
                }
            }

            throw new System.Exception("No scene found");
        }
    }
}
