﻿
//====================================================================
//** Copyright © classbao.com 2016 -- QQ:1165458780 -- 请保留此注释 **
//====================================================================
// 文件名称：DateHelper.cs
// 项目名称：常用方法实用工具集
// 创建时间：2016年7月25日10时59分
// ===================================================================
using System;
using System.Collections.Generic;
namespace M.Core.Utils
{
    /// <summary>
    /// 日期操作辅助类
    /// </summary>
    public static class DateHelper
    {

        #region 时间转换
        /// <summary>
        /// 转换时间为unix时间戳
        /// </summary>
        /// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        #endregion


        #region 时间格式检测
        ///// <summary>
        ///// 判断字符串是否是yy-mm-dd字符串
        ///// </summary>
        ///// <param name="str">待判断字符串</param>
        ///// <returns>判断结果</returns>
        //public static bool IsDateString(string str)
        //{
        //    return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        //}

        #endregion

        #region 年
        /// <summary>
        /// 计算某年共有多少天
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <returns>共有多少天</returns>
        public static int YearOfTotalDay(int year)
        {
            if (year <= 0001 || year >= 9999)
            {
                return -1;
            }

            return YearOfLastDay(year).DayOfYear;
        }
        /// <summary>
        /// 获得某年第一天的日期
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <returns>第一天日期</returns>
        public static DateTime YearOfFirstDay(int year)
        {
            if (year <= 0001 || year >= 9999)
            {
                return DateTime.MaxValue;
            }

            DateTime result = DateTime.MinValue;
            DateTime.TryParse(string.Format("{0}-01-01", year), out result);
            return result;
        }
        /// <summary>
        /// 获得某年最后一天的日期
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <returns>最后一天日期</returns>
        public static DateTime YearOfLastDay(int year)
        {
            if (year <= 0001 || year >= 9999)
            {
                return DateTime.MaxValue;
            }

            DateTime result = DateTime.MaxValue;
            DateTime.TryParse(string.Format("{0}-12-31", year), out result);
            return result;
        }

        #endregion

        #region 月

        /// <summary>
        /// 获得当前年当前月的总天数
        /// </summary>
        /// <returns>共有多少天</returns>
        public static int MonthOfTotalDay()
        {
            DateTime _now = DateTime.Now;
            return DateTime.DaysInMonth(_now.Year, _now.Month);
        }
        /// <summary>
        /// 获得某年某月的总天数
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <param name="month">需要计算的月份</param>
        /// <returns>共有多少天</returns>
        public static int MonthOfTotalDay(int year, int month)
        {
            if (year <= 0001 || year >= 9999)
            {
                return -1;
            }

            if (month < 1 || month > 12)
            {
                return -1;
            }

            return DateTime.DaysInMonth(year, month);
        }
        #endregion

        #region 周

        /// <summary>
        /// 周（星期）信息实体类
        /// </summary>
        [Serializable]
        public class WeekInfo
        {
            /// <summary>
            /// 周（星期）信息实体类
            /// </summary>
            public WeekInfo()
            {
                Number = 0;
                BeginDate = DateTime.MinValue;
                EndDate = DateTime.MaxValue;
            }
            /// <summary>
            /// 周数
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime BeginDate { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime EndDate { get; set; }
            /// <summary>
            /// 输出第几周和日期间隔
            /// <para>默认格式：</para>
            /// <para>第几周，从2012年12月21日至2112年12月21日。</para>
            /// </summary>
            /// <returns>第几周和日期间隔</returns>
            public override string ToString()
            {
                return string.Format("第{0}周，从{1}至{2}。", Number, BeginDate.ToString("yyyy年MM月dd日"), EndDate.ToString("yyyy年MM月dd日"));
            }
            /// <summary>
            /// 输出第几周
            /// </summary>
            /// <param name="isFill">少于2位时是否补零</param>
            /// <returns>第几周</returns>
            public string GetWeekString(bool isFill)
            {
                string _format = "第{0}周";
                if (isFill && Number < 10)
                {
                    _format = "第0{0}周";
                }

                return string.Format(_format, Number);
            }
            /// <summary>
            /// 输出日期间隔
            /// </summary>
            /// <param name="inputString">输出格式化字符串</param>
            /// <param name="dateString">日期格式化字符串</param>
            /// <returns>日期间隔</returns>
            public string GetDateString(string inputString, string dateString)
            {
                if (string.IsNullOrWhiteSpace(inputString) || string.IsNullOrWhiteSpace(dateString))
                {
                    return null;
                }

                try
                {
                    return string.Format(inputString, BeginDate.ToString(dateString), EndDate.ToString(dateString));
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 获得年度第一个周一的日期
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
        /// <param name="offset">年度第一个周一偏移量</param>
        /// <returns>年度第一个周一的日期</returns>
        public static DateTime WeekOfFirstDay(int year, bool jumpYear, out int offset)
        {
            DateTime firstDate = YearOfFirstDay(year); //该年的第一天
            int firstWeek = (int)firstDate.DayOfWeek; //该年的第一天是周几
            offset = 0; //周一偏移量
            switch (firstWeek)
            {
                case 1: //星期一
                    offset = jumpYear ? 0 : 0;
                    break;
                case 2: //星期二
                    offset = jumpYear ? 6 : -1;
                    break;
                case 3: //星期三
                    offset = jumpYear ? 5 : -2;
                    break;
                case 4: //星期四
                    offset = jumpYear ? 4 : -3;
                    break;
                case 5: //星期五
                    offset = jumpYear ? 3 : -4;
                    break;
                case 6: //星期六
                    offset = jumpYear ? 2 : -5;
                    break;
                case 0: //星期日
                    offset = jumpYear ? 1 : -6;
                    break;
            }
            firstDate = firstDate.AddDays(offset);
            return firstDate;
        }
        /// <summary>
        /// 获得年度周（星期）信息实体集合列表
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
        /// <returns>周（星期）信息实体集合列表对象</returns>
        public static IList<WeekInfo> WeekOfList(int year, bool jumpYear)
        {
            IList<WeekInfo> weekList = new List<WeekInfo>();
            if (year <= 0001 || year >= 9999)
            {
                return weekList;
            }

            int offset = 0;
            DateTime firstDate = WeekOfFirstDay(year, jumpYear, out offset); //年度周一的日期
            int index = 1;
            WeekInfo weekInfo;
            while (true)
            {
                if (index > 54)
                {
                    break;
                }

                weekInfo = new WeekInfo();
                weekInfo.Number = index;
                weekInfo.BeginDate = firstDate; //周一
                weekInfo.EndDate = firstDate.AddDays(6); //周日
                weekList.Add(weekInfo);
                firstDate = firstDate.AddDays(7); //下周
                if (jumpYear)
                {
                    if (firstDate.Year != year)
                    {
                        break;
                    }
                }
                else
                {
                    if (firstDate.AddDays(6).Year != year)
                    {
                        break;
                    }
                }
                index++;
            }
            return weekList;
        }
        /// <summary>
        /// 获得某年第某周的开始日期和结束日期
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <param name="weekNumber">需要计算的周数</param>
        /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
        /// <param name="weekBeginDate">开始日期</param>
        /// <param name="weekEndDate">结束日期</param>
        public static void WeekOfDate(int year, int weekNumber, bool jumpYear, out DateTime weekBeginDate, out DateTime weekEndDate)
        {
            weekBeginDate = DateTime.MinValue;
            weekEndDate = DateTime.MaxValue;
            if (year <= 0001 || year >= 9999 || weekNumber < 1 || weekNumber > 54)
            {
                return;
            }

            int offset = 0;
            DateTime firstDate = WeekOfFirstDay(year, jumpYear, out offset); //年度周一的日期
            firstDate = firstDate.AddDays((weekNumber - 1) * 7);
            weekBeginDate = firstDate;
            weekEndDate = firstDate.AddDays(6);
        }
        /// <summary>
        /// 获得某个日期属于某年的第几周
        /// </summary>
        /// <param name="currentDate">需要计算的日期</param>
        /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
        /// <returns>某年的第几周</returns>
        public static int WeekOfCurrent(DateTime currentDate, bool jumpYear)
        {
            if (DateTime.MinValue == currentDate || DateTime.MaxValue == currentDate)
            {
                return 0;
            }

            int result = 0, offset = 0;
            DateTime firstDate = WeekOfFirstDay(currentDate.Year, jumpYear, out offset); //年度周一的日期
            int firstWeek = (int)firstDate.DayOfWeek; //该年的第一天是周几
            int currentDay = currentDate.DayOfYear;
            if (offset > 0)
            {
                currentDay += offset;
            }
            else
            {
                currentDay -= offset;
            }
            int remainderDay = currentDay - (7 - firstWeek);
            if (remainderDay < 1)
            {
                result = 1;
            }
            else
            {
                result = remainderDay / 7;
                if (remainderDay % 7 != 0)
                {
                    result++;
                }

                result++;
            }
            return result;
        }
        /// <summary>
        /// 统计一段时间内有多少个星期几
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="weekNumber">星期几</param>
        /// <returns>多少个星期几</returns>
        public static int WeekOfTotalWeeks(DateTime beginDate, DateTime endDate, DayOfWeek weekNumber)
        {
            TimeSpan _dayTotal = new TimeSpan(endDate.Ticks - beginDate.Ticks);
            int result = (int)_dayTotal.TotalDays / 7;
            double iLen = _dayTotal.TotalDays % 7;
            for (int i = 0; i <= iLen; i++)
            {
                if (beginDate.AddDays(i).DayOfWeek == weekNumber)
                {
                    result++;
                    break;
                }
            }
            return result;
        }

        #endregion

        #region 季度

        /// <summary>
        /// 计算当前月属于第几季度
        /// </summary>
        /// <returns>当前年第几季度</returns>
        public static int QuarterOfCurrent()
        {
            return QuarterOfCurrent(DateTime.Now.Month);
        }
        /// <summary>
        /// 计算某个月属于第几季度
        /// </summary>
        /// <param name="month">需要计算的月份</param>
        /// <returns>某年第几季度</returns>
        public static int QuarterOfCurrent(int month)
        {
            if (month < 1 || month > 12)
            {
                return -1;
            }

            return (month - 1) / 3 + 1;
        }
        /// <summary>
        /// 获得当前年当前季度的开始日期和结束日期
        /// </summary>
        /// <param name="quarterBeginDate">开始日期</param>
        /// <param name="quarterEndDate">结束日期</param>
        public static void QuarterOfDate(out DateTime quarterBeginDate, out DateTime quarterEndDate)
        {
            int quarter = QuarterOfCurrent(DateTime.Now.Month);
            QuarterOfDate(DateTime.Now.Year, quarter, out quarterBeginDate, out quarterEndDate);
        }
        /// <summary>
        /// 获得指定日期所在季度的开始日期和结束日期
        /// </summary>
        /// <param name="fromDate">需要计算的日期</param>
        /// <param name="quarterBeginDate">开始日期</param>
        /// <param name="quarterEndDate">结束日期</param>
        public static void QuarterOfDate(DateTime fromDate, out DateTime quarterBeginDate, out DateTime quarterEndDate)
        {
            int quarter = QuarterOfCurrent(fromDate.Month);
            QuarterOfDate(fromDate.Year, quarter, out quarterBeginDate, out quarterEndDate);
        }
        /// <summary>
        /// 获得某年第某季度的开始日期和结束日期
        /// </summary>
        /// <param name="year">需要计算的年份</param>
        /// <param name="quarter">需要计算的季度</param>
        /// <param name="quarterBeginDate">开始日期</param>
        /// <param name="quarterEndDate">结束日期</param>
        public static void QuarterOfDate(int year, int quarter, out DateTime quarterBeginDate, out DateTime quarterEndDate)
        {
            quarterBeginDate = DateTime.MinValue;
            quarterEndDate = DateTime.MaxValue;
            if (year <= 0001 || year >= 9999 || quarter < 1 || quarter > 4)
            {
                return;
            }

            int month = (quarter - 1) * 3 + 1;
            quarterBeginDate = new DateTime(year, month, 1);
            quarterEndDate = quarterBeginDate.AddMonths(3).AddMilliseconds(-1);
        }

        #endregion
    }
}
