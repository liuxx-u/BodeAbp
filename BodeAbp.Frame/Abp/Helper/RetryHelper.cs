using System;

namespace Abp.Helper
{
    /// <summary>
    /// 重试操作辅助类
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// 重试
        /// </summary>
        /// <param name="func">重试方法</param>
        /// <param name="times">重试次数</param>
        /// <returns>是否执行成功</returns>
        public static bool Retry(Func<bool> func, int times)
        {
            while (times > 0)
            {
                if (func()) return true;
                times--;
            }
            return false;
        }
    }
}
