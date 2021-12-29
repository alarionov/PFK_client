using UnityEngine.Serialization;

namespace PFK
{
    [System.Serializable]
    public class Character
    {
        public string ContractAddress;
        public int TokenId;
        public Equipment Equipment;
        public BaseStats Stats;
        public int Upgrades;
        public int Exp;
        public int Level;
    }
}