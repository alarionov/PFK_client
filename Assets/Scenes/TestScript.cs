using PFK;
using UnityEngine;
using WordBearers;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        BaseStats stats1 = new BaseStats() {Armour = 1, Health = 2, Attack = 3};
        BaseStats stats2 = new BaseStats() {Armour = 1, Health = 2, Attack = 3};
        BaseStats stats3 = new BaseStats() {Armour = 1, Health = 2, Attack = 4};
        
        Debug.Log($"s1 == s2 {stats1.Equals(stats2)}");
        Debug.Log($"s1 == s3 {stats1.Equals(stats3)}");
        Debug.Log($"s2 == s3 {stats2.Equals(stats3)}");
    }
}
