using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CryptoAppTwo
{
    public class AesObject
    {
        // ПОЛЯ

        public byte[] TextInByte = new byte[0]; // Входной массив байтов
        public TypeDisplay TextInType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary
        public byte[] TextOutByte = new byte[0]; // ВЫходной массив байтов
        public TypeDisplay TextOutType = TypeDisplay.None; // 0 - symbol, 1 - Hex, 2 - binary
        public byte[] KeyByte = new byte[0]; // Ключ
        public byte[] IVByte = new byte[0]; // вектор IV
        public TypeDisplay KeyType = TypeDisplay.None; // Режим вывода ключа 0 - symbol, 1 - Hex, 2 - binary

        public bool EncryptOrDecrypt = true;
        public string FileExtension = "txt";  // Расширение считанного файла

        public bool TextInIsEdited = false;
        public bool TextOutIsEdited = false;
        public bool KeyIsEdited = false;
        //================================================================================================

        public void Encrypt()
        {
            try
            {
                this.TextOutByte = AesObject.Encrypt(this.TextInByte, this.KeyByte, this.IVByte);
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }
            //return this.TextOutByte;
        }

        public void Decrypt()
        {
            try
            {
                this.TextOutByte = AesObject.Decrypt(this.TextInByte, this.KeyByte, this.IVByte);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            //return this.TextOutByte;
        }

        public static byte[] Encrypt(byte[] msg, byte[] key, byte[] iv)
        {
            byte[] byteOut = new byte[0];
            ICryptoTransform cryptoTransform = null;
            AesCng aescng = null;

            try
            {
                aescng = new AesCng(); // объект класса у алгоритма AES
                aescng.Key = key; // присваиваем ключ из аргумента
                aescng.IV = iv; // присваиваем вектор из аргумента
                // создали объект-расшифратор
                cryptoTransform = aescng.CreateEncryptor();
                // получили байты на выходе
                byteOut = cryptoTransform.TransformFinalBlock(msg, 0, msg.Length);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                aescng.Dispose(); // освобождаем ресурсы
                cryptoTransform.Dispose(); // освобождаем ресурсы
            }
            return byteOut;
        }

        public static byte[] Decrypt(byte[] cipher, byte[] key, byte[] iv)
        {
            byte[] byteOut = new byte[0];
            ICryptoTransform cryptoTransform = null;
            AesCng aescng = null;

            try
            {
                aescng = new AesCng(); // объект класса у алгоритма AES
                aescng.Key = key; // присваиваем ключ из аргумента
                aescng.IV = iv; // присваиваем вектор из аргумента
                // создали объект-расшифратор
                cryptoTransform = aescng.CreateDecryptor();
                // получили байты на выходе
                byteOut = cryptoTransform.TransformFinalBlock(cipher, 0, cipher.Length);
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                aescng.Dispose(); // освобождаем ресурсы
                cryptoTransform.Dispose(); // освобождаем ресурсы
            }
            return byteOut;
        }
    }
}
