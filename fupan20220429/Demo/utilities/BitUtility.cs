using System;

namespace Demo.utilities
{
    class BitUtility
    {
        public static bool GetBitValue(int value, int index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); 
            var val = 1 << index;
            return (value & val) == val;
        }

        public static int SetBitValue(int value, int index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); 
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }
    }
}
