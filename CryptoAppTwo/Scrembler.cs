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
        public int greatestDegreePolynom = 1;
        public int maxSizePolynom = 11;


        public Scrembler()
        {
            polynom = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            numberStart = 0;
        }

        public static byte[] generatorLFSR(ref Scrembler scrembler, int sizeTextInByte)
        {
            byte[] key = new byte[0];
            int size = scrembler.polynom.Length; // количество бит (размер полинома)
            int greatestDegreePolynom = size;
            // ищем наибольший разряд
            for (int i = size; i > 0; i--) // этот фор если от младшего до старшего записаны
            {
                if (scrembler.polynom[i - 1] == 1)
                {
                    greatestDegreePolynom = i;
                    break;
                }
            }
            /*for (int i = 0; i < size; i++) // этот фор поиска если от старшего до младшего записаны
            {
                if (scrembler.polynom[i] == 1)
                {
                    greatestDegreePolynom = i;
                    break;
                }
            }*/

            string binNumberStart = Convert.ToString(scrembler.numberStart, 2).PadLeft(size, '0');
            List<int> listNumberStart = new List<int>();
            foreach (char c in binNumberStart)
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

        // критерий Хи2
        public static double checkCriteryHiSquare(string seq)
        {
            int n = seq.Length;
            int z = 0, o = 0;
            for (int i = 0; i < n; i++)
                if (seq[i] == '0')
                    z++;
                else
                    o++;
            double s = 2.0D * ((double)(n));
            s *= Math.Pow(((double)z) / ((double)n) - 0.5, 2.0) + Math.Pow(((double)o) / ((double)n) - 0.5, 2.0);
            return s;
        }

        // период скремблера
        public static int periodScrembler(string seq)
        {
            string toCheck = seq.Substring(seq.Length - 16);
            int per = 1;
            for (int i = seq.Length - 17; i >= 0; i--)
            {
                string x = seq.Substring(i, 16);
                if (x == toCheck)
                    break;
                per++;
            }
            return per;
        }

        // поиск периода последовательности
        public static int findPeriod(string sequence)
        {
            int begin = 0, periodLength = 1;
            int beginStart = -1;
            int oldPeriod = periodLength;

            //do
            {
                beginStart++;
                oldPeriod = periodLength;
                do
                {
                    if (sequence[begin] == sequence[periodLength + begin])
                    {
                        begin++;
                    }
                    else
                    {
                        begin = beginStart;
                        periodLength++;
                    }
                } while (begin + periodLength != sequence.Length);
            }
            //while (beginStart + periodLength != sequence.Length/* && periodLength > oldPeriod*/);

            return periodLength;
        }

        // сбалансированность
        public static bool balance(string seq, ref double rez_sb)
        {
            bool flag = true;
            int interval = 1000;
            int index = 0;
            int n = seq.Length;


            // Разбиваем ее на интервалы по 1000 символов
            for (int j = 0; flag && index < n; j++)
            {
                int z = 0, o = 0;

                for (int i = j * interval; flag && i < interval * (j + 1) && i < seq.Length; i++)
                {
                    index++;
                    // Для этого интервала считаем количества нулей и единиц
                    if (seq[i] == '0')
                        z++;
                    else
                        o++;
                }
                // Если количество равно, то последовательность считается сбалансированной
                rez_sb = (double)Math.Abs(z - o) / interval;
                if (rez_sb > 0.05)
                    flag = false;
            }
            return flag;
        }

        // корреляция
        public static double korr(string seq)
        {
            bool flag = true;
            int pl = 0;
            int mi = 0;
            int n = seq.Length;
            int sdvig = (int)(n * 0.05); // ограничение последовательности, шаг для проверки
            for (int i = sdvig; i < n - sdvig; i++)
            {
                if (seq[i] == seq[i + sdvig])
                    // одинаковые элементы
                    pl++;
                else
                    // разные элементы
                    mi++;
            }

            if ((double)Math.Abs(pl - mi) / (n - sdvig - sdvig) > 0.05)
                flag = false;

            return (double)Math.Abs(pl - mi) / (n - sdvig - sdvig);
        }
    }
}
