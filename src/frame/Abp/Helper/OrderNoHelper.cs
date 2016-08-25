using System;

namespace Abp.Helper
{
    /// <summary>
    /// 订单号生成帮助类
    /// </summary>
    public class OrderNoHelper
    {
        /// <summary>
        /// 防止创建类的实例
        /// </summary>
        private OrderNoHelper() { }

        private static readonly object Locker = new object();
        private static int _sn = 0;

        /// <summary>
        /// 生成订单编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateId(string code)
        {
            lock (Locker)  //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。
            {
                if (_sn == 999999)
                {
                    _sn = 0;
                }
                else
                {
                    _sn++;
                }
                return code + DateTime.Now.ToString("yyyyMMddHHmmss") + _sn.ToString().PadLeft(6, '0');
            }
        }
    }
}
