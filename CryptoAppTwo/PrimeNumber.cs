using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CryptoAppTwo
{
    public class PrimeNumber
    {
        // Класс для хранения одного простого числа, затраченных времени кол-ва итераций на генерацию его
        public BigInteger Number = BigInteger.One;
        public int TimeSec = 0; // в секундах
        public int IterCount = 0;

        PrimeNumber(BigInteger number, int timeSec, int iterCount)
        {
            this.Number = number;
            this.TimeSec = timeSec;
            this.IterCount = iterCount;
        }
    }


}
