using DotNetBlog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBlog.Identity
{
    public static class UserManager
    {
        public static string GetSalt(int length = 5)
        {
            string strRandomStr = "0123456789AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(strRandomStr[rnd.Next(strRandomStr.Length)]);
            }
            string strSalt = sb.ToString();
            return strSalt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSalt"></param>
        /// <param name="password">the encrypted base64string password</param>
        /// <returns></returns>
        public static string GetPasswordHash(string strSalt, string password)
        {
            RSA rsa = RSAExtensions.CreateRsaFromPrivateKey(RSAConstants.PrivateKey);
            var cipherBytes = System.Convert.FromBase64String(password);
            var plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
            //var planText = Encoding.UTF8.GetString(plainTextBytes);
            var hashedTextBytes = Encoding.UTF8.GetBytes(strSalt).Concat(plainTextBytes).ToArray();

            MD5 md5 = MD5.Create();
            var byteMd5Pwd = md5.ComputeHash(hashedTextBytes);
            var strMd5Pwd = BitConverter.ToString(byteMd5Pwd).Replace("-", "");
            return strMd5Pwd;
        }
    }
}
