using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    class Gamirovanie
    {
        public byte[] GamTextInByte; // Входной массив байтов
        public TypeDisplay GamTextInType = TypeDisplay.Symbol; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] GamTextOutByte; // ВЫходной массив байтов
        public int GamTextOutType = 1; // 0 - symbol, 1 - Hex, 2 - binary

        public byte[] GamKeyByte; // Ключ
        public TypeDisplay GamKeyType = TypeDisplay.Symbol; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary
        public bool GamKeyIsEntry = false; // Введенн ли ключ или нет

        public bool GamEncryptOrDecrypt = true; // Режим либо шифруем либо ДЕшифруем
        
        public string GamFileExtension = "";  // Расширение считанного файла

        public bool GamIsEdited = false;
    }
}
