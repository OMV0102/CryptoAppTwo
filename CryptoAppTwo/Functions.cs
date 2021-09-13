﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    public static class Functions
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public static Byte[] readAllBytesFromFile(String fileName)
        {
            return null;
        }

        public static bool checkSymbolIsHex(int numberChar)
        {
            // цифры                                       заглавные A-F                                строчные a-f
            if ((numberChar >= 48 && numberChar <= 57) || (numberChar >= 65 && numberChar <= 70) || (numberChar >= 97 && numberChar <= 102))
                return true;
            else
                return false;
        }

        // Hex to Byte
        public static byte[] HexToByte(string strHEX)
        {
            strHEX = strHEX.Replace("-", "").ToUpper();
            int N = strHEX.Length;
            int step = 2;
            byte[] bytes = new byte[N / step];
            for (int i = 0, j = 0; i < N; i += step, j++)
                bytes[j] = Convert.ToByte(strHEX.Substring(i, step), 16);
            return bytes;
        }

        // Byte to Hex
        public static string ByteToHex(byte[] byteArr)
        {
            return BitConverter.ToString(byteArr).ToUpper();
        }

        // Symbol to Byte
        public static byte[] SymbolToByte(string str)
        {
            int N = str.Length;
            byte[] bytes = new byte[N];
            for (int i = 0; i < N; i++)
                bytes[i] = Convert.ToByte(str[i]);

            return bytes;
        }

        // Byte to Symbol
        public static string ByteToSymbol(byte[] byteArr)
        {
            return Encoding.UTF8.GetString(byteArr);
        }

        // Binary to Byte
        public static byte[] BinaryToByte(string strBin)
        {
            strBin = strBin.Replace("-", "");
            int N = strBin.Length;
            int step = 8;
            byte[] bytes = new byte[N / step];
            for (int i = 0, j = 0; i < N; i += step, j++)
                bytes[j] = Convert.ToByte(strBin.Substring(i, step), 2);
            return bytes;
        }

        // Byte to Binary
        public static string ByteToBinary(byte[] byteArr)
        {
            string strBin = "";
            int N = byteArr.Length;
            int step = 8;
            string temp = "";
            for(int i = 0; i < N-1; i++)
            {
                strBin += Convert.ToString(byteArr[i], 2).PadLeft(step, '0') + "-";
            }
            strBin += Convert.ToString(byteArr[N - 1], 2).PadLeft(step, '0');
            return strBin;
        }

        //=============================================================
        // функции для остальных методов (deprecated)
        // Переводит 16-ричную строку в байты
        public static byte[] StringHexToByteArray(string strHEX)
        {
            int N = strHEX.Length;
            byte[] bytes = new byte[N / 2];
            for (int i = 0; i < N; i += 2)
                bytes[i / 2] = Convert.ToByte(strHEX.Substring(i, 2), 16);
            return bytes;
        }

        // Переводит байты в значение 16 ричной строки
        public static string ByteArrayTOStringHex(byte[] byteArr)
        {
            return BitConverter.ToString(byteArr).Replace("-", "").ToUpper();
        }
        //=============================================================
    }
}
