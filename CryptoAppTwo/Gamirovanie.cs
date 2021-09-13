using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    class Gamirovanie
    {
        public byte[] TextInByte; // Входной массив байтов
        public TypeDisplay TextInType = TypeDisplay.Symbol; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] TextOutByte; // ВЫходной массив байтов
        public TypeDisplay TextOutType = TypeDisplay.Symbol; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] KeyByte; // Ключ
        public TypeDisplay KeyType = TypeDisplay.Symbol; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary
        public bool KeyIsEntry = false; // Введен ли ключ или нет

        public bool EncryptOrDecrypt = true; // Режим либо шифруем либо ДЕшифруем

        public string FileExtension = "";  // Расширение считанного файла

        public bool TextInIsEdited = false;
        public bool TextOutIsEdited = false;
    }
}
