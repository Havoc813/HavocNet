using System;
using System.Text;
using System.Security.Cryptography;

namespace Core
{
    public static class Crypto
    {
        public static string Encrypt(string strPassword)
        {
            return StandardEncrypt(CustomEncrypt(strPassword));
        }

        public static string CustomEncrypt(string strPassword)
        {
            var strKey = "moldoveanu".ToCharArray();
            var strNewPass = ""; 
            var strPassArray = strPassword.ToCharArray();

            for(var i = 0; i < strPassArray.Length; i++)
            {
                strNewPass += (char)((strPassArray[i] + strKey[i % strKey.Length]) % 126);
            }

            return strNewPass;
        }

        public static string StandardEncrypt (string strPassword )
        {
            var ue = new UnicodeEncoding();
            var md5 = new MD5CryptoServiceProvider();

            return Convert.ToBase64String(md5.ComputeHash(ue.GetBytes(strPassword)));
        }

        public static string RandomPassword(int length)
        {
            var j = new Random();
            var pass = "";

            for (var i = 0; i < length; i++)
            {
                pass += (char) (j.Next() % 94 + 33);
            }

            return pass;
        }
    }
}
