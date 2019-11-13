using System;

namespace M.Core.Extensions
{
    public static class OjectExtensions
    {
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt(this object data)
        {
            if (data == null)
            {
                return 0;
            }

            if (data is bool)
            {
                return (bool)data ? 1 : 0;
            }
            var success = int.TryParse(data.ToString(), out int result);
            if (success)
            {
                return result;
            }

            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Double.</returns>
        public static double ToDouble(this object data)
        {
            if (data == null)
            {
                return 0;
            }

            return double.TryParse(data.ToString(), out double result) ? result : 0;
        }

        /// <summary>
        /// 转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        /// <returns>System.Double.</returns>
        public static double ToDouble(this object data, int digits)
        {
            return Math.Round(ToDouble(data), digits, System.MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ToDecimal(this object data)
        {
            if (data == null)
            {
                return 0;
            }

            return decimal.TryParse(data.ToString(), out decimal result) ? result : 0;
        }

        /// <summary>
        /// 转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? ToDecimalOrNull(this object data)
        {
            if (data == null)
            {
                return null;
            }

            bool isValid = decimal.TryParse(data.ToString(), out decimal result);
            if (isValid)
            {
                return result;
            }

            return null;
        }
    }
}
