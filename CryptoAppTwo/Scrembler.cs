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

        public static byte[] generatorLFSR(ref Scrembler scrembler, int sizeTextInByte)
        {
            byte[] key = new byte[0];
            int size = scrembler.polynom.Length; // количество бит (размер полинома)
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

            string binNumberStart = Convert.ToString(scrembler.numberStart, 2).PadLeft(size, '0');
            List<int> listNumberStart = new List<int>();
            foreach(char c in binNumberStart)
            {
                listNumberStart.Add(Int32.Parse(c.ToString()));
            }

            List<int> listPolynom = new List<int>();
            listPolynom.AddRange(scrembler.polynom.AsEnumerable<int>());

            int requiredQuantity = sizeTextInByte * 8;// кол-во бит необходимых сгенерировать
            string sequenceBit = Scrembler.RegenNum(requiredQuantity, listPolynom, listNumberStart);
            key = Functions.BinaryToByte(sequenceBit);

            return key;
        }

        public static string RegenNum(int requiredQuantity, List<int> listPolynom, List<int> binNumberStart)
        {
            StringBuilder nums = new StringBuilder();
            int bit = 0;

            int period = 0;
            int sizePolynom = listPolynom.Count;
            do
            {
                // Ищем измененный новый бит
                bit = 0;
                for (int i = 0; i < sizePolynom; i++)
                    // Сложение по модулю 2 двух веторов - двоичных чисел
                    bit = bit ^ ((listPolynom[i] == 1 && binNumberStart[i] == 1) ? 1 : 0);

                for (int i = sizePolynom - 2; i >= 0; i--)
                    binNumberStart[i + 1] = binNumberStart[i]; // Смещаем биты влево
                binNumberStart[0] = bit; // Ставим измененный новый бит			  
                nums.Append(bit.ToString());
                period++;
            } while (period < requiredQuantity);

            return nums.ToString();
        }
    }
}
