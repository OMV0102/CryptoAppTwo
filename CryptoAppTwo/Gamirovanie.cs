using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppTwo
{
    class Gamirovanie
    {
        public byte[] GamByteIn; // Входной массив байтов
        public byte[] GamByteOut; // ВЫходной массив байтов
        public byte[] GamByteKey; // Ключ
        public bool GamEncryptOrDecrypt = true; // Режим либо шифруем либо ДЕшифруем
        public bool GamKeyIsEntry = false; // Введенн ли ключ или нет
        public string GamFileExtension = "";  // Расширение считанного файла
    }
}
