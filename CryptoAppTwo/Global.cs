using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Drawing;

namespace CryptoAppTwo
{
    public static class Global
    {
        public static byte[] Hesh_byte_in; // Входной массив байтов для хэширования
        //=================================
        public static byte[] Simm_byte_in; // Входной массив байтов для Симметричного ШИФРОВАНИЯ
        public static byte[] Simm_byte_out; // ВЫходной массив байтов после Симметричного ШИФРОВАНИЯ
        public static byte[] Simm_byte_key; // Ключ для Симметричного ШИФРОВАНИЯ
        public static byte[] Simm_byte_iv; // Вектор инициализации для Симметричного ШИФРОВАНИЯ
        public static bool Simm_EncryptOrDecrypt; // Режим либо шифруем либо расшифровываем для Симметричного ШИФРОВАНИЯ
        public static bool Simm_KeyIV_isEntry; // Введенны ли ключ и  вектор или нет для Симметричного ШИФРОВАНИЯ
        public static string Simm_file_extension = "";  // Расширение считанного файла данных для Симметричного ШИФРОВАНИЯ
        //=====================================
        public static byte[] Asim_byte_in; // Входной массив байтов для Асимметричного ШИФРОВАНИЯ
        public static byte[] Asim_byte_out; // ВЫходной массив байтов после Асимметричного ШИФРОВАНИЯ
        public static int Asim_size_key_bit = 2048;  // Размер ключа в битах для rsa (2048 бит = 256 байт указан макс. размер)
        public static int Asim_size_key_byte = Asim_size_key_bit / 8;  // Размер ключа в байтах для rsa 
        public static byte[] Asim_byte_key;  // Ключ для Асимметричного ШИФРОВАНИЯ
        public static string Asim_file_key = "";   // путь файла с ключом для Асимметричного ШИФРОВАНИЯ
        public static bool Asim_EncryptOrDecrypt; // Режим либо шифруем либо расшифровываем для Асимметричного ШИФРОВАНИЯ
        public static bool Asim_Keys_isEntry; // Введенн ли ключ или нет для Асимметричного ШИФРОВАНИЯ
        public static string Asim_file_extension = "";  // Расширение считанного файла данных для Асимметричного ШИФРОВАНИЯ
        //=====================================
        public static int eds_size_key_bit = 2048;  // Размер ключа в битах для rsa (2048 бит = 256 байт указан макс. размер)
        public static int eds_size_key_byte = eds_size_key_bit / 8;  // Размер ключа в байтах для rsa 
        public static byte[] eds_byte_message; // Входной массив байтов сообщения
        public static byte[] eds_byte_sign; // ВЫходной массив ЭЦП
        public static byte[] eds_byte_key;  // Ключ для ЭЦП
        public static string eds_file_key = ""; // путь файла с ключом для ЭЦП
        public static bool eds_signORcheck; // Режим либо подписываем либо проверяем
        public static bool eds_Keys_isEntry; // Введенн ли ключ или нет для ЭЦП
        public static bool eds_data_isSign; // Успешно ли прошло создание ЭЦП или нет
        public static bool eds_data_isCheck; // Успешно ли прошла проверка ЭЦП или нет
    }
}
