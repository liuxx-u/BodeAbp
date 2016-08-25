using System;

namespace Abp.Helper
{
    /// <summary>
    /// DateTime帮助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 根据出生年月获取年龄
        /// </summary>
        /// <param name="birthDay">出生年月</param>
        /// <returns>年龄</returns>
        public static int GetAge(DateTime birthDay) 
        {
            DateTime now = DateTime.Today;

            int age = now.Year - birthDay.Year;

            if (birthDay > now.AddYears(-age)) age--;

            return age;
        }

        /// <summary>
        /// 根据出生年月获取年龄
        /// </summary>
        /// <param name="birthDay">出生年月</param>
        /// <returns>年龄</returns>
        public static int GetAge(string birthDay) 
        {
            try 
            {
                var bday = DateTime.Parse(birthDay);
                return GetAge(bday);
            }
            catch 
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取本周开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetWeekStartTime()
        {
            return GetWeekStartTime(DateTime.Today);
        }

        /// <summary>
        /// 获取指定日期所在周开始时间
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <returns></returns>
        public static DateTime GetWeekStartTime(DateTime dt)
        {
            DateTime day = dt.Date;
            return day.DayOfWeek == DayOfWeek.Sunday
                ? day.AddDays(-6)
                : day.AddDays(1 - Convert.ToInt32(day.DayOfWeek.ToString("d")));
        }

        /// <summary>
        /// 获取本周结束时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetWeekEndTime()
        {
            return GetWeekEndTime(DateTime.Today);
        }

        /// <summary>
        /// 获取指定日期所在周结束时间
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <returns></returns>
        public static DateTime GetWeekEndTime(DateTime dt)
        {
            DateTime weekStartTime = GetWeekStartTime(dt);
            return weekStartTime.AddDays(7).AddMinutes(-1);
        }

        /// <summary>
        /// 获取本月开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthStartTime()
        {
            return GetMonthStartTime(DateTime.Today);
        }

        /// <summary>
        /// 获取指定日期所在月的开始时间
        /// </summary>
        /// <param name="dt">指定日期</param>
        /// <returns></returns>
        public static DateTime GetMonthStartTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }


        /// <summary>
        /// 获取本月结束时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthEndTime()
        {
            return GetMonthEndTime(DateTime.Today);
        }

        /// <summary>
        /// 获取指定日期所在月的结束时间
        /// </summary>
        /// <param name="dt">指定日期</param>
        /// <returns></returns>
        public static DateTime GetMonthEndTime(DateTime dt)
        {
            DateTime monthStartTime = GetMonthStartTime(dt);
            return monthStartTime.AddMonths(1).AddMinutes(-1);
        }
        
        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }
    }
}
