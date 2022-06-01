using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Net.Util.Helper
{
    public static class CommonHelper
    {

        #region 获取现在是星期几
        /// <summary>
        /// 获取现在是星期几
        /// </summary>
        /// <returns></returns>
        public static string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        #endregion

        #region 获取32位md5加密
        /// <summary>
        /// 通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns>32位md5加密字符串</returns>
        public static string Md5For32(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }
        #endregion

        #region String数组转Int数组
        public static int[] StringArrAyToIntArray(string[] str)
        {
            try
            {
                int[] iNums = Array.ConvertAll<string, int>(str, s => int.Parse(s));
                return iNums;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region string 转int数组

        public static int[] StringToIntArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new int[0];
                if (str.EndsWith(","))
                {
                    str = str.Remove(str.Length - 1, 1);
                }
                var idstrarr = str.Split(',');
                var idintarr = new int[idstrarr.Length];

                for (int i = 0; i < idstrarr.Length; i++)
                {
                    idintarr[i] = Convert.ToInt32(idstrarr[i]);
                }
                return idintarr;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion


        #region 从字典中取单个数据
        /// <summary>
        /// 从字典中取单个数据
        /// </summary>
        /// <param name="configs"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static string GetConfigDictionary(Dictionary<string, DictionaryKeyValues> configs, string skey)
        {
            configs.TryGetValue(skey, out var di);
            return di?.sValue;
        }

        #endregion
    }
}
