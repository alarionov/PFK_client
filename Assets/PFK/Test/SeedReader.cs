using UnityEngine;

namespace PFK.Test
{
    public class SeedReader : MonoBehaviour
    {
        [SerializeField] private string _seed;

        [ContextMenu("Check Seed")]
        public void CheckSeed()
        {
            Debug.Log($"Seed: {_seed}");
            PFK.SeedReader seed = new PFK.SeedReader(_seed);
        }
    }
}
