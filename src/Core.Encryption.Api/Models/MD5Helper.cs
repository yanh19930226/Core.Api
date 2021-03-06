using System.Security.Cryptography;

namespace Core.Encryption.Api.Models
{
    public static class MD5Helper
    {
        /// <summary>
        ///计算字符串的md5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeMd5(this string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return ComputeMd5(bytes);
        }

        /// <summary>
        /// 计算byte[]的md5值
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ComputeMd5(this byte[] bytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }
    }
}
