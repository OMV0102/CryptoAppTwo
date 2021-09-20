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
        public TypeDisplay TextInType = TypeDisplay.Hex; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] TextOutByte = new byte[0]; // ВЫходной массив байтов
        public TypeDisplay TextOutType = TypeDisplay.Hex; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] KeyByte = new byte[0]; // Ключ
        public TypeDisplay KeyType = TypeDisplay.None; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary
        public bool KeyIsEntry = false; // Введен ли ключ или нет
        public bool KeyIsCorrect = false; // Сгенерирован ли ключ автоматически или нет

        public bool EncryptOrDecrypt = true; // Режим либо шифруем либо ДЕшифруем

        public string FileExtension = "txt";  // Расширение считанного файла

        public bool TextInIsEdited = false;
        public bool TextOutIsEdited = false;
        public bool KeyIsEdited = false;

        public Gamirovanie() { }

        public Gamirovanie(byte[] KeyByte, TypeDisplay KeyType, bool KeyIsEntry, bool KeyIsCorrect)
        {
            this.KeyByte = KeyByte;
            this.KeyType = KeyType;
            this.KeyIsEntry = KeyIsEntry;
            this.KeyIsCorrect = KeyIsCorrect;
        }

    }

}
