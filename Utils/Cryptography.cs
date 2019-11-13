using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace M.Core.Utils
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class Cryptography
    {
        static readonly string key = "M.Core911";
        static readonly string iv = "M.Core911";
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">加密值</param>
        /// <returns></returns>
        public static string Encrypt(string encryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(iv);
            byte[] valueToEncrypt = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(valueToEncrypt, 0, valueToEncrypt.Length);
            cStream.FlushFinalBlock();

            StringBuilder result = new StringBuilder();
            foreach (byte b in mStream.ToArray())
            {
                result.AppendFormat("{0:X2}", b);
            }

            cStream.Close();
            mStream.Close();

            return result.ToString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">解密值</param>
        /// <returns></returns>
        public static string Decrypt(string decryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(iv);
            byte[] inputByteArray = new byte[decryptString.Length / 2];
            for (int x = 0; x < decryptString.Length / 2; x++)
            {
                int i = (Convert.ToInt32(decryptString.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            string result = Encoding.UTF8.GetString(mStream.ToArray());

            cStream.Close();
            mStream.Close();

            return result;
        }
        /// <summary>
        /// 内部加密
        /// </summary>
        /// <param name="encryptString">加密值</param>
        /// <returns></returns>
        public static string InnerEncrypt(string encryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(iv);
            byte[] valueToEncrypt = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(valueToEncrypt, 0, valueToEncrypt.Length);
            cStream.FlushFinalBlock();

            string result = Convert.ToBase64String(mStream.ToArray());

            cStream.Close();
            mStream.Close();

            return result.ToString();
        }
        /// <summary>
        /// 内部解密
        /// </summary>
        /// <param name="decryptString">解密值</param>
        /// <returns></returns>
        public static string InnerDecrypt(string decryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(iv);
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            string result = Encoding.UTF8.GetString(mStream.ToArray());

            cStream.Close();
            mStream.Close();

            return result;
        }
    }
}

