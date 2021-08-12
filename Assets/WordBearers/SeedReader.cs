using System.Linq;

namespace WordBearers
{
    public class SeedReader
    {
        private byte[] _rolls;
        private int index;
        
        public SeedReader(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new System.Exception("Odd length of the seed hex string");
            }
            
            index = 0;
            _rolls = new byte[hexString.Length / 2];
            
            for (int i = 0; i < hexString.Length/2; ++i)
            {
                _rolls[_rolls.Length - 1 - i] = 
                    System.Convert.ToByte(
                        hexString.Substring(i*2, 2), 16);
            }
        }

        public int Roll(byte d)
        {
            return _rolls[index++] % d;
        }
    }
}