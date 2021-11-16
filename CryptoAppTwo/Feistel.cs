using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    public class Feistel
    {
        public enum KeyMethodGenerate : int // перечисление методы генерации ключа
        {
            Cycle = 0,
            Scrambler = 1,
            None = -1,
        }

        public enum FunctionMethodGenerate : int  // вид образующей функции
        {
            Xor = 0,
            Single = 1,
            None = -1,
        }

        // ПОЛЯ
        public KeyMethodGenerate SubKeyMode = KeyMethodGenerate.Cycle;
        public FunctionMethodGenerate FuncMode = FunctionMethodGenerate.Single;

        public byte[] TextInByte = new byte[0]; // Входной массив байтов
        public TypeDisplay TextInType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary
        public byte[] TextOutByte = new byte[0]; // ВЫходной массив байтов
        public TypeDisplay TextOutType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary
        public byte[] KeyByte = new byte[0]; // Ключ
        public TypeDisplay KeyType = TypeDisplay.None; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary

        public bool EncryptOrDecrypt = true;
        public string FileExtension = "txt";  // Расширение считанного файла

        public bool TextInIsEdited = false;
        public bool TextOutIsEdited = false;
        public bool KeyIsEdited = false;

        public int ChartBitsChanging = 10;
        public List<int> ChartListBitsText = new List<int>(); // для графика 1
        public List<int> ChartListBitsKey = new List<int>(); // для графика 1
        //================================================================================================
        // приватные поля для функций
        private const int rounds = 16;
        private List<BitArray> roundsBitArrayList = new List<BitArray>(rounds);
        //==============================

        public List<byte> Encrypt(byte[] TextIn, byte[] Key)
        {
            roundsBitArrayList.Clear();

            List<byte> result = new List<byte>();

            int blocksNum = TextIn.Length / 8;
            int i;

            for (i = 0; i < blocksNum; i++)
            {
                byte[] block1 = new byte[4];
                byte[] block2 = new byte[4];

                for (int j = 0; j < 4; j++)
                {
                    block1[j] = TextIn[i * 8 + j];
                }

                for (int j = 4; j < 8; j++)
                {
                    block2[j - 4] = TextIn[i * 8 + j];
                }

                // применим сеть Фейстеля
                BitArray block1BitArray = new BitArray(block1);
                BitArray block2BitArray = new BitArray(block2);
                BitArray keyBitArray = new BitArray(Key);

                for (int j = 0; j < rounds; j++)
                {
                    Round(keyBitArray, ref block1BitArray, ref block2BitArray, j);
                }
                // размен последнего раунда
                block1BitArray.CopyTo(block2, 0);
                block2BitArray.CopyTo(block1, 0);
                result.AddRange(block1);
                result.AddRange(block2);

            }
            int mod = TextIn.Length % 8;
            if (mod != 0)
            {
                byte[] block1 = new byte[4] { 0, 0, 0, 0 };
                byte[] block2 = new byte[4] { 0, 0, 0, 0 };

                for (int j = 0; j < 4 && j < mod; j++)
                {
                    block1[j] = TextIn[i * 8 + j];
                }

                for (int j = 4; j < 8 && j < mod; j++)
                {
                    block2[j - 4] = TextIn[i * 8 + j];
                }

                // применим сеть Фейстеля
                BitArray block1BitArray = new BitArray(block1);
                BitArray block2BitArray = new BitArray(block2);
                BitArray keyBitArray = new BitArray(Key);

                for (int j = 0; j < rounds; j++)
                {
                    Round(keyBitArray, ref block1BitArray, ref block2BitArray, j);
                }

                // размен последнего раунда
                block1BitArray.CopyTo(block2, 0);
                block2BitArray.CopyTo(block1, 0);
                result.AddRange(block1);
                result.AddRange(block2);

            }

            return result;
        }

        public List<byte> Decrypt(byte[] TextIn, byte[] Key) // функция дешифрования шифротекста
        {
            List<byte[]> blocks = new List<byte[]>();
            List<byte> result = new List<byte>();

            int blocksNum = TextIn.Length / 8;
            int i;

            for (i = 0; i < blocksNum; i++)
            {
                byte[] block1 = new byte[4];
                byte[] block2 = new byte[4];

                for (int j = 0; j < 4; j++)
                {
                    block1[j] = TextIn[i * 8 + j];
                }

                for (int j = 4; j < 8; j++)
                {
                    block2[j - 4] = TextIn[i * 8 + j];
                }

                // применим сеть Фейстеля
                BitArray block1BitArray = new BitArray(block1);
                BitArray block2BitArray = new BitArray(block2);
                BitArray keyBitArray = new BitArray(Key);

                for (int j = rounds; j > 0; j--)
                {
                    Round(keyBitArray, ref block1BitArray, ref block2BitArray, j - 1);
                }

                // размен последнего раунда
                block1BitArray.CopyTo(block2, 0);
                block2BitArray.CopyTo(block1, 0);
                result.AddRange(block1);
                result.AddRange(block2);

            }
            int mod = TextIn.Length % 8;
            if (mod != 0)
            {
                byte[] block1 = new byte[4] { 0, 0, 0, 0 };
                byte[] block2 = new byte[4] { 0, 0, 0, 0 };

                for (int j = 0; j < 4 && j < mod; j++)
                {
                    block1[j] = TextIn[i * 8 + j];
                }

                for (int j = 4; j < 8 && j < mod; j++)
                {
                    block2[j - 4] = TextIn[i * 8 + j];
                }

                // применим сеть Фейстеля
                BitArray block1BitArray = new BitArray(block1);
                BitArray block2BitArray = new BitArray(block2);
                BitArray keyBitArray = new BitArray(Key);

                for (int j = 0; j < rounds; j++)
                {
                    Round(keyBitArray, ref block1BitArray, ref block2BitArray, j);
                }

                // размен последнего раунда
                block1BitArray.CopyTo(block2, 0);
                block2BitArray.CopyTo(block1, 0);
                result.AddRange(block1);
                result.AddRange(block2);

            }

            return result;
        }

        private void SwapBitArray(ref BitArray a1, ref BitArray a2)
        {
            BitArray temp = new BitArray(a1.Length);
            temp = a1;
            a1 = a2;
            a2 = temp;
        }

        private void Round(BitArray keyBitArray, ref BitArray block1BitArray, ref BitArray block2BitArray, int roundNumber)
        {
            BitArray subKeyBitArray = new BitArray(32);

            byte[] Fbytes = new byte[4];

            if (this.SubKeyMode == KeyMethodGenerate.Cycle)
            {
                for (int k = 0; k < 32; k++) // подключ для раунда
                {
                    subKeyBitArray[k] = keyBitArray[(roundNumber + k) % keyBitArray.Count];
                }
            }
            else if (this.SubKeyMode == KeyMethodGenerate.Scrambler)
            {
                //bool[] polynom = new bool[] { false, false, false, false, false, false, true, true };
                int[] polynom = new int[] { 0, 0, 0, 0, 0, 0, 1, 1 };

                BitArray initialValue = new BitArray(8);

                for (int k = 0; k < 8; k++) // начальное значение скремблера
                {
                    initialValue[k] = keyBitArray[(roundNumber + k) % keyBitArray.Count];
                }

                //byte[] byteInitialValue = new byte[1];
                //initialValue.CopyTo(byteInitialValue, 0);
                Scrembler scrembler = new Scrembler();
                scrembler.numberStart = 0;
                scrembler.polynom = polynom;

                //subKeyBitArray = new BitArray(Scrambler.LSFR(polynom, byteInitialValue, 32));
                subKeyBitArray = new BitArray(Scrembler.generatorLFSR(ref scrembler, 4));
            }

            if (this.FuncMode == FunctionMethodGenerate.Single)
            {
                subKeyBitArray.CopyTo(Fbytes, 0);
            }
            else if (this.FuncMode == FunctionMethodGenerate.Xor)
            {
                //bool[] polynom = new bool[] { false, true, false, false,
                //                              false, false, false, false,
                //                              false, false, false, false,
                //                              false, false, true, true};
                int[] polynom = new int[] { 0, 1, 0, 0,
                                              0, 0, 0, 0,
                                              0, 0, 0, 0,
                                              0, 0, 1, 1};
                BitArray initialValue = new BitArray(16);
                Scrembler scrembler = new Scrembler();
                scrembler.polynom = polynom;

                for (int k = 0; k < 16; k++) // начальное значение скремблера
                {
                    initialValue[k] = keyBitArray[(roundNumber + k) % keyBitArray.Count];
                }

                //byte[] byteInitialValue = new byte[2];
                //initialValue.CopyTo(byteInitialValue, 0);
                int[] array = new int[1];
                initialValue.CopyTo(array, 0);
                scrembler.numberStart = array[0];

                //F = Scrambler.LSFR(polynom, byteInitialValue, 32);
                Fbytes = Scrembler.generatorLFSR(ref scrembler, 4);
                BitArray xor = new BitArray(Fbytes);
                xor.Xor(block1BitArray);
                xor.Xor(subKeyBitArray);
                xor.CopyTo(Fbytes, 0);

            }

            block2BitArray.Xor(new BitArray(Fbytes));

            this.SwapBitArray(ref block2BitArray, ref block1BitArray);

            BitArray tmp = new BitArray(64);

            for (int i = 0; i < 32; i++)
            {
                tmp[i] = block1BitArray[i];
                tmp[i + 32] = block2BitArray[i];
            }

            roundsBitArrayList.Add(tmp);
        }

    }
}
