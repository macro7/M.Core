using System;

namespace M.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 格式化成时间
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="dateFormatType">Type of the date format.</param>
        /// <returns><c>true</c> if [is date time] [the specified s]; otherwise, <c>false</c>.</returns>
        public static string ToDateFormat(this DateTime s, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return s.ToString(format);
        }

        /// <summary>
        /// 获取本周第一天日期
        /// 星期天为第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentWeekFirstDay(this DateTime dateTime)
        {
            try
            {
                return dateTime.AddDays(Convert.ToDouble(0 - Convert.ToInt16(dateTime.DayOfWeek)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取本周最后一天日期
        /// DateTime.Now.AddDays(7).ToShortDateString();//7天后
        ///DateTime.Now.AddDays(-7).ToShortDateString();7天前
        /// 周六为最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentWeekLastDay(this DateTime dateTime)
        {
            try
            {
                return dateTime.AddDays(Convert.ToDouble(6 - Convert.ToInt16(dateTime.DayOfWeek)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 用数组处理获取本周当日为星期几
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentWeekDayString(this DayOfWeek dayOfWeek)
        {
            try
            {
                string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                return Day[Convert.ToInt16(dayOfWeek)];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取本个月的第一天
        /// DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "1";
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentMonthFirstDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, +dateTime.Month, 1);
        }

        /// <summary>
        /// 获取本个月的最后一天
        /// 这个处理有异常.
        /// DateTime.Parse(DateTime.Now.Year.ToString() +
        /// DateTime.Now.Month.ToString() + "1").AddMonths(1).AddDays(-1).ToShortDateString();
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentMonthLastDay(this DateTime dateTime)
        {
            ////建议使用这个
            return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获取本年的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentYearFirstDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        /// <summary>
        /// 获取本年的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentYearLastDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1).AddYears(1).AddDays(-1);
        }
    }
}
