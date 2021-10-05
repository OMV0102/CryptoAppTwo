using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    public class Scrembler
    {
        public int numberStart = 0;
        public int[] polynom = null;


        public Scrembler()
        {
            polynom = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            numberStart = 0;
        }

        public static byte[] generatorLFSR(ref Scrembler scrembler)
        {
            byte[] key = new byte[0];
            int size = scrembler.polynom.Length;
            int index = size;
            // ищем наибольший разряд
            for (int i = size; i > 0; i--)
            {
                if (scrembler.polynom[i - 1] == 1)
                {
                    index = i;
                    break;
                }
            }

            string bin = Convert.ToString(scrembler.numberStart, 2).PadLeft(10, '0');
            lf.getNum(num);
            lf.MakePolynom(firstButton.Checked ? 0 : 1);

            int count;
            if (radioButtonSymb.Checked)
                count = sourceTextBox.Text.Count() * 8;
            else
                if (radioButtonHex.Checked)
                count = sourceTextBox.Text.Count() * 2;
            else
                count = sourceTextBox.Text.Count();
            byte[] key = Scrambler.binToByte(lf.RegenNum(count));

            keyTextBox.Text = byteToCustomType(key, true);

            return key;
        }


    }
}
