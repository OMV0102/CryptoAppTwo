using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    public class Scrembler
    {
        public int numberStart = 1;
        public int[] polynom = null;
        private int greatestDegreePolynom = 1;
        //public int maxSizePolynom = 10;


        public Scrembler()
        {
            polynom = new int[9] { 0, 0, 0, 0, 0, 1, 0, 0, 1 };
            numberStart = 1;
        }

        public static byte[] generatorLFSR(ref Scrembler scrembler, int requiredCountByte)
        {
            byte[] key = new byte[0];
            int size = scrembler.polynom.Length; // количество бит (размер полинома)
            scrembler.greatestDegreePolynom = size-1;
            // ищем наибольший разряд
            for (int i = size; i > 0; i--) // этот фор если от младшего до старшего записаны слева направо
            {
                if (scrembler.polynom[i - 1] == 1)
                {
                    scrembler.greatestDegreePolynom = i;
                    break;
                }
            }
            /*for (int i = 0; i < size; i++) // этот фор поиска если от старшего до младшего записаны слева направо
            {
                if (scrembler.polynom[i] == 1)
                {
                    greatestDegreePolynom = size-i;
                    break;
                }
            }*/

            string binNumberStart = Convert.ToString(scrembler.numberStart, 2).PadLeft(scrembler.greatestDegreePolynom, '0');
            //перевернуть биты начального значения (чтобы были от младшего до старшего)
            string sequenceReverse = "";
            for (int i = binNumberStart.Length - 1; i >= 0; i--)
            {
                sequenceReverse += binNumberStart[i];
            }
            binNumberStart = sequenceReverse;
            //биты из строки в биты списка целых
            List<int> listNumberStart = new List<int>();
            foreach (char c in binNumberStart)
            {
                listNumberStart.Add(Int32.Parse(c.ToString()));
            }

            List<int> listPolynom = new List<int>();
            listPolynom.AddRange(scrembler.polynom.AsEnumerable<int>());

            int requiredQuantity = requiredCountByte * 8;// кол-во бит необходимых сгенерировать
            string sequenceBit = Scrembler.RegenNum(requiredQuantity, listPolynom, listNumberStart, scrembler.greatestDegreePolynom);
            key = Functions.BinaryToByte(sequenceBit);

            return key;
        }

        public static string RegenNum(int requiredQuantity, List<int> listPolynom, List<int> binNumberStart, int greatestDegreePolynom)
        {
            StringBuilder nums = new StringBuilder();
            int bit = 0;

            int period = 0;
            int sizePolynom = listPolynom.Count;
            
            do
            {
                // Ищем измененный новый бит
                bit = 0;
                for (int i = 0; i < greatestDegreePolynom; i++)
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
        public static int findPeriod0(string seq)
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
        public static int findPeriod1(string sequence)
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

        public static int findPeriod2(string sequence)
        {
            string sequenceReverse = "";
            for(int i = sequence.Length-1; i > 0; i--)
            {
                sequenceReverse += sequence[i];
            }
            sequence = sequenceReverse;

            int begin = 0, periodLength = 1;
            int beginStart = -1;
            int oldPeriod = periodLength;

            for(int i = 0; i < sequence.Length-5; i++)
            {
                for (int j = i+3; j < sequence.Length - 2; j++)
                {
                    if (sequence[i] == sequence[j] && sequence[i+2] == sequence[j+2])
                    {
                        periodLength = j - i;
                    }
                }
            }

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
                    // Для  интервала считаем количества 0 и 1
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
