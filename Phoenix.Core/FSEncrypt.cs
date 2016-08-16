using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Phoenix.Core
{
    public class FSEncrypt
    {
        //private const string PassPhrase = "Pas5pr@se";
        private const string PassPhrase = "Fr0mThe@shes";
        //private const string SaltValue = "s@1tValue";
        private const string SaltValue = "R!sesThePHOENIX";
        private const int PasswordIterations = 2;
        private const string InitVector = "@1B2c3D4e5F6g7H8";
        private const int KeySize = 256;
        private static readonly byte[] InitVectorBytes = Encoding.ASCII.GetBytes(InitVector);
        private static readonly byte[] SaltValueBytes = Encoding.ASCII.GetBytes(SaltValue);


        public static string Encrypt(string plainText)
        {
            if (String.IsNullOrEmpty(plainText)) return "";

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(PassPhrase, SaltValueBytes, PasswordIterations);
            byte[] keyBytes = password.GetBytes(KeySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            //string cipherText = Convert.ToBase64String(cipherTextBytes);
            //return cipherText;
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText)) return "";

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(PassPhrase, SaltValueBytes, PasswordIterations);
            byte[] keyBytes = password.GetBytes(KeySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            //string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            //return plainText;
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
