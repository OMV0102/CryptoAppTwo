using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    public class Gamirovanie
    {
        public byte[] TextInByte = new byte[0]; // Входной массив байтов
        public TypeDisplay TextInType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] TextOutByte = new byte[0]; // ВЫходной массив байтов
        public TypeDisplay TextOutType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] KeyByte = new byte[0]; // Ключ
        public TypeDisplay KeyType = TypeDisplay.None; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary
        public bool KeyIsEntry = false; // Введен ли ключ или нет
        public bool KeyIsCorrect = false; // Сгенерирован ли ключ автоматически или нет

        public bool EncryptOrDecrypt = true; // Режим либо шифруем либо ДЕшифруем

        public bool GamirovanieOrScrembler = true; // Гамирование или скремблирвание

        public string FileExtension = "txt";  // Расширение считанного файла

        public bool TextInIsEdited = false;
        public bool TextOutIsEdited = false;
        public bool KeyIsEdited = false;

        public Scrembler scrembler = null;

        public Gamirovanie() 
        {
            scrembler = new Scrembler();
        }

        public Gamirovanie(byte[] KeyByte, TypeDisplay KeyType, bool KeyIsEntry, bool KeyIsCorrect)
        {
            this.KeyByte = KeyByte;
            this.KeyType = KeyType;
            this.KeyIsEntry = KeyIsEntry;
            this.KeyIsCorrect = KeyIsCorrect;

            scrembler = new Scrembler();
        }


        // функция для ГАММИРОВАНИЯ
        // аргументы: вход. байты, байты ключа, Байты на выходе или сообщение об ошибке
        public static bool GamirovanieXOR(byte[] textIn, byte[] key, out byte[] textOut, out string message)
        {
            textOut = new byte[0];
            message = "";

            if (textIn.Length < 1)
            {
                message = "Входные байты имеют нулевую длину!";
                return false;
            }

            if (key.Length < 1)
            {
                message = "Ключ имеет нулевую длину!";
                return false;
            }

            if (textIn.Length != key.Length)
            {
                message = "Количество входных байт и ключа не совпадает!";
                return false;
            }

            textOut = new byte[textIn.Length];
            int N = textOut.Length;
            for (int i = 0; i < N; i++)
            {
                textOut[i] = (byte)(textIn[i] ^ key[i]);
            }

            return true;
        }

        // функция Скремблирования
        // аргументы: вход. байты, байты ключа, Байты на выходе
        public static bool Scremblirovanie(byte[] textIn, byte[] key, out byte[] textOut, out string message)
        {
            textOut = new byte[0];
            message = "";

            if (textIn.Length < 1)
            {
                message = "Входные байты имеют нулевую длину!";
                return false;
            }

            if (key.Length < 1)
            {
                message = "Ключ имеет нулевую длину!";
                return false;
            }

            if (textIn.Length != key.Length)
            {
                message = "Количество входных байт и ключа не совпадает!";
                return false;
            }

            textOut = new byte[textIn.Length];
            int N = textOut.Length;
            for (int i = 0; i < N; i++)
            {
                textOut[i] = (byte)(textIn[i] ^ key[i]);
            }

            return true;
        }
    }

}
