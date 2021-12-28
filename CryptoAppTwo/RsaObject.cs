using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CryptoAppTwo
{
    public class RsaObject
    {
        // ПОЛЯ

        public byte[] TextInByte = new byte[0]; // Входной массив байтов
        public TypeDisplay TextInType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary
        public byte[] TextOutByte = new byte[0]; // ВЫходной массив байтов
        public TypeDisplay TextOutType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary
        public byte[] KeyByte = new byte[0]; // Ключ
        public byte[] KeyByteRsa = new byte[0]; // Ключ
        public byte[] KeyByteEncoded = new byte[0]; // Ключ
        public TypeDisplay KeyType = TypeDisplay.None; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary

        public bool EncryptOrDecrypt = true;
        public string FileExtension = "txt";  // Расширение считанного файла

        public bool TextInIsEdited = false;
        public bool TextOutIsEdited = false;
        public bool KeyIsEdited = false;
        //===================================


        //================================================================================================

    }
}
