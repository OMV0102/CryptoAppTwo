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
    public static class Algorithms
    {
        // функция для хэширования массива байт заданным алгоритмом, на выходе 16-ричная строка
        // аргументы: входные байты сообщения, алгоритм хэширования
        public static string HeshAlg(Byte[] arrayByte_in, string selectedAlgHesh)
        {
            byte[] arrayByte_out = new byte[0]; // Выходная последовательность байт после хеширования
            string heshString = ""; // Хеш строка (16 ричный вид)

            try
            {

                switch (selectedAlgHesh) // Получение хеша определенным алгоритмом
                {
                    case "MD5":
                        MD5 md5 = MD5.Create();
                        arrayByte_out = md5.ComputeHash(arrayByte_in);
                        md5.Dispose();
                        break;

                    case "RIPEMD160":
                        RIPEMD160 ripemd160 = RIPEMD160.Create();
                        arrayByte_out = ripemd160.ComputeHash(arrayByte_in);
                        ripemd160.Dispose();
                        break;

                    case "SHA1":
                        SHA1 sha1 = SHA1.Create();
                        arrayByte_out = sha1.ComputeHash(arrayByte_in);
                        sha1.Dispose();
                        break;

                    case "SHA256":
                        SHA256 sha256 = SHA256.Create();
                        arrayByte_out = sha256.ComputeHash(arrayByte_in);
                        sha256.Dispose();
                        break;

                    case "SHA384":
                        SHA384 sha384 = SHA384.Create();
                        arrayByte_out = sha384.ComputeHash(arrayByte_in);
                        sha384.Dispose();
                        break;

                    case "SHA512":
                        SHA512 sha512 = SHA512.Create();
                        arrayByte_out = sha512.ComputeHash(arrayByte_in);
                        sha512.Dispose();
                        break;

                    default: break;
                }
                heshString = BitConverter.ToString(arrayByte_out).Replace("-", "");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return heshString;
        }

        // функция для СИММ. Шифрования массива байт заданным алгоритмом и режимом, на выходе массив байт
        // аргументы: вход. байты; ключ; вектор инициализации; алгоритм AES / 3DES; режим шифруем / дешифруем
        public static Byte[] SimmAlg(Byte[] arrayByte_in, Byte[] key, Byte[] iv, string selectedAlgSimm, bool EncryptIsTrue)
        {
            byte[] arrayByte_out = new byte[0]; // Выходная последовательность байт после шифрования/расшифровки
            ICryptoTransform cryptoTransform;

            try
            {
                switch (selectedAlgSimm) // Получение хеша определенным алгоритмом
                {
                    case "AES":
                        AesCng aescng = new AesCng(); // объект класса у алгоритма AES
                        aescng.Key = key; // присваиваем ключ из аргумента
                        aescng.IV = iv; // присваиваем вектор из аргумента
                        if (EncryptIsTrue == true) // если вызвали для ШИФРования AES
                        {
                            // создали объект-шифратор
                            cryptoTransform = aescng.CreateEncryptor();
                        }
                        else  // если вызвали для РАСшифровки AES
                        {
                            // создали объект-расшифратор
                            cryptoTransform = aescng.CreateDecryptor();
                        }
                        // получили байты на выходе
                        arrayByte_out = cryptoTransform.TransformFinalBlock(arrayByte_in, 0, arrayByte_in.Length);

                        aescng.Dispose(); // освобождаем ресурсы
                        cryptoTransform.Dispose(); // освобождаем ресурсы
                        break;

                    case "3DES":
                        TripleDESCng tripledescng = new TripleDESCng(); // объект класса у алгоритма AES
                        tripledescng.Key = key; //  присваиваем ключ из аргумента
                        tripledescng.IV = iv; //  присваиваем вектор из аргумента
                        if (EncryptIsTrue == true) // если вызвали для ШИФРования AES
                        {
                            // создали объект-шифратор
                            cryptoTransform = tripledescng.CreateEncryptor();
                        }
                        else  // если вызвали для РАСшифровки 3DES
                        {
                            // создали объект-расшифратор
                            cryptoTransform = tripledescng.CreateDecryptor();
                        }
                        // получили байты на выходе
                        arrayByte_out = cryptoTransform.TransformFinalBlock(arrayByte_in, 0, arrayByte_in.Length);

                        tripledescng.Dispose(); // освобождаем ресурсы
                        cryptoTransform.Dispose(); // освобождаем ресурсы
                        break;

                    default: break;
                }
            }
            catch (Exception error)
            {
                //MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (EncryptIsTrue == true) // если шифрование
                    MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Ключ или вектор инициализации не подходят", "Ошибка шифрования", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Заданные ключ или вектор инициализации не подходят для заданного шифра!\nРасшифровка не возможна.", "Ошибка расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return arrayByte_out;
        }

        // функция для Асим. Шифрования 
        // аргументы: вход. байты; ключ; ; алгоритм RSA; режим шифруем / дешифруем
        public static Byte[] AsimAlg(Byte[] arrayByte_in, Byte[] key, string selectedAlgSimm, bool EncryptIsTrue)
        {
            byte[] arrayByte_out = new byte[0]; // Выходная последовательность байт после шифрования/расшифровки

            try
            {
                switch (selectedAlgSimm) // Получение хеша определенным алгоритмом
                {
                    case "RSA":
                        RSACryptoServiceProvider rsacrypto = new RSACryptoServiceProvider(Global.Asim_size_key_bit);  // объект класса у алгоритма RSA
                        rsacrypto.ImportCspBlob(key); // присваиваем ключ из аргумента

                        if (EncryptIsTrue == true) // если вызвали для ШИФРования AES
                        {
                            try
                            {
                                arrayByte_out = rsacrypto.Encrypt(arrayByte_in, RSAEncryptionPadding.OaepSHA1);
                            }
                            catch
                            {
                                throw new Exception("Возникла ошибка при шифровании!\nВозмоджно данные слишком велики размером...");
                            }
                        }
                        else  // если вызвали для РАСшифровки AES
                        {
                            try
                            {
                                arrayByte_out = rsacrypto.Decrypt(arrayByte_in, RSAEncryptionPadding.OaepSHA1);
                            }
                            catch
                            {
                                throw new Exception("Введенный ключ не подходит для заданного шифра!\nили\nВыбранны НЕзашифрованные данные.\nРасшифровка возможна только секретным ключом!");
                            }
                        }
                        // получили байты на выходе

                        rsacrypto.Dispose(); // освобождаем ресурсы
                        break;

                    default: break;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return arrayByte_out;
        }

        // функция для создания ЭЦП
        // аргументы: вход. байты сообщения; ключ (приватный)
        public static Byte[] edsAlg_signData(Byte[] message, Byte[] key)
        {
            byte[] sign_out = new byte[0]; // Выходная последовательность байт  ЭЦП

            try
            {
                try
                {
                    if (key.Length < Global.eds_size_key_byte)
                    {
                        throw new Exception("Введенный ключ не подходит этому приложению! (Не соответствие размера)");
                    }
                }
                catch
                {
                    throw;
                }
                RSACryptoServiceProvider rsaeds = new RSACryptoServiceProvider(Global.eds_size_key_bit);  // объект класса у алгоритма RSA
                // ввод ключа
                try
                {
                    rsaeds.ImportCspBlob(key); // присваиваем ключ из аргумента
                }
                catch
                {
                    rsaeds.Dispose();
                    throw new Exception("Произошла ошибка при импортировании ключа в метод RSA!");
                }
                // само создание подписи
                try
                {
                    sign_out = rsaeds.SignData(message, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1); // получаем ЭЦП в байтах
                    Global.eds_data_isSign = true;
                }
                catch
                {
                    rsaeds.Dispose();
                    throw new Exception("Произошла ошибка при подписи данных!\nПроверьте правильность введенного ключа.\nПодписать можно только секретным ключом!\n\nТакже, возможно объем данных слишком велик для RSA...");
                }
                rsaeds.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sign_out;
        }

        // функция для ПРОВЕРКИ ЭЦП
        // аргументы: вход. байты сообщения; ключ (приватный/публичный), сформированная подпись
        public static bool edsAlg_verifyData(Byte[] message, Byte[] key, Byte[] sign)
        {
            bool result_check = false; // На выходе либо ДА либо НЕТ

            try
            {
                try
                {
                    if (key.Length < Global.eds_size_key_byte)
                    {
                        throw new Exception("Введенный ключ не подходит этому приложению! (Не соответствие размера)");
                    }
                }
                catch
                {
                    throw;
                }
                try
                {
                    if (sign.Length < Global.eds_size_key_byte)
                    {
                        throw new Exception("Формат заданной подписи не подходит этому приложению! (Не соответствие размера)");
                    }
                }
                catch
                {
                    throw;
                }

                RSACryptoServiceProvider rsaeds = new RSACryptoServiceProvider(Global.eds_size_key_bit);  // объект класса у алгоритма RSA
                // ввод ключа
                try
                {
                    rsaeds.ImportCspBlob(key); // присваиваем ключ из аргумента
                }
                catch
                {
                    rsaeds.Dispose();
                    throw new Exception("Произошла ошибка при импортировании ключа в метод RSA!");
                }
                // проверка подписи для исходного сообщения
                try
                {
                    result_check = rsaeds.VerifyData(message, sign, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                }
                catch
                {
                    rsaeds.Dispose();
                    throw new Exception("Произошла ошибка при проверке подписи!\nПроверьте правильность введенных данных:\n> Файл с данными;\n> Ключ\n> Подпись, которую проверяем для сообщения;");
                }
                rsaeds.Dispose();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result_check;
        }

        // функция для ГАМИРОВАНИЯ // AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        // аргументы: вход. байты, байты ключа, Байты на выходе
        public static bool GamirovanieAlgorithm(byte[] textIn, byte[] key, out byte[] textOut, string message)
        {
            textOut = new byte[0];

            if(textIn.Length < 1)
            {
                message = "Сообщение имеет нулевую длину!";
                return false;
            }

            if (key.Length < 1)
            {
                message = "Сообщение имеет нулевую длину!";
                return false;
            }

            if (textIn.Length != key.Length)
            {
                message = "Количество входных байт и ключа не совпадает!";
                return false;
            }

            textOut = new byte[textIn.Length];
            int N = textOut.Length;
            for(int i = 0; i < N; i++)
            {
                textOut[i] = (byte)(textIn[i] ^ key[i]);
            }

            return true;
        }
    }
}
