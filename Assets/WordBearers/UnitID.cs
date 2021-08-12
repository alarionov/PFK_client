using UnityEngine;

namespace WordBearers
{
    public enum UnitIDEnum
    {
        // Heroe
        Player = 0,
        
        // Enemies
        Weak = 1,
        Quick = 2,
        Thick = 3
    }

    public class UnitID : MonoBehaviour
    {
        public UnitIDEnum ID;
    }
}
