using System;
using System.Text;

namespace M.Core.Extensions
{
    public static class StringExtensions
    {
        #region 时间格式
        /// <summary>
        /// 校验字符串是否是时间格式
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static bool IsDateTime(this string s)
        {
            DateTime result;
            return DateTime.TryParse(s, out result);
        }

        /// <summary>
        /// 校验字符串是否是Int类型
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static bool IsInt(this string s)
        {
            return int.TryParse(s, out int result);
        }

        /// <summary>
        /// 校验字符串是否是float类型
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static bool IsDecimal(this string s)
        {
            decimal result;
            return decimal.TryParse(s, out result);
        }

        /// <summary>
        /// Double
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static bool IsDouble(this string s)
        {
            double result;
            return double.TryParse(s, out result);
        }

        /// <summary>
        /// 转化成Decimal
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static decimal? ToDecimal(this string s)
        {
            if (decimal.TryParse(s, out decimal result))
            {
                return result;
            }
            return null;
        }


        /// <summary>
        /// 转化成Double
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static double? ToDouble(this string s)
        {
            if (double.TryParse(s, out double result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// 转化成bool
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static bool ToBool(this string s)
        {
            if (s.Trim().ToLower().Equals("true") || s.Trim() == "1")
            {
                return true;
            }
            else if (s.Trim().ToLower().Equals("false") || s.Trim() == "0")
            {
                return false;
            }
            else
            {
                if (bool.TryParse(s, out bool result))
                {
                    return result;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 去掉后面的0，如1.50 -> 1.5, 100.00 -> 100
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is date time] [the specified is date time]; otherwise, <c>false</c>.</returns>
        public static string WithoutEndZero(this string s)
        {
            if (decimal.TryParse(s, out decimal result))
            {
                return result.ToString("G0");
            }
            else
            {
                return s;
            }
        }


        /// <summary>
        /// 返回yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s)
        {
            if (DateTime.TryParse(s, out DateTime dt))
            {
                return dt;
            }
            return null;
        }

        #region 按字节计算字符串
        /// <summary>
        /// 截取长度区分中文，下标从0开始
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="iStrart">开始</param>
        /// <param name="iLength">获取长度</param>
        /// <returns>返回字符串</returns>
        public static string RealMid(this string str, int iStrart, int iLength)
        {
            string rStr = "";
            try
            {
                int len = 0;
                len = 0;
                rStr = "";
                for (int j = 0; j < str.Length; j++)
                {
                    if (len >= iStrart && len < iStrart + iLength)
                    {
                        rStr = rStr + str[j].ToString();
                    }
                    if (str[j] >= 0x3000 && str[j] <= 0x9FFF)
                    {
                        len = len + 2;
                    }
                    else
                    {
                        len = len + 1;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rStr;
        }

        /// <summary>
        /// * 中国、日本和韩国的象形文字（总称为CJK）占用了从0x3000到0x9FFF的代码
        ///* 希腊字母表使用从0x0370到0x03FF的代码
        ///* 斯拉夫语使用从0x0400到0x04FF的代码
        ///* 美国使用从0x0530到0x058F的代码
        ///* 希伯来语使用从0x0590到0x05FF的代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static int RealLen(string str)
        {
            int nLength = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 0x3000 && str[i] <= 0x9FFF)
                {
                    nLength += 2;
                }
                else
                {
                    nLength++;
                }
            }
            return nLength;
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// 根据配置对指定字符串进行 MD5 加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMD5(this string s)
        {
            //md5加密
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(s));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
    }
}