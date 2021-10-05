using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    public class Scrembler
    {
        int numberStart = 0;
        int[] polynom = null;


        public Scrembler()
        {
            polynom = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            numberStart = 0;
        }

        public static byte[] generatorLFSR(ref Scrembler scrembler)
        {
            byte[] key = new byte[0];
            return key;
        }


    }
}
