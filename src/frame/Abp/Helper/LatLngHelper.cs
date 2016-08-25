using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Helper
{
    public static class LatLngHelper
    {
        private const double EarthRadius = 6378.137;//地球半径

        /// <summary>
        /// 获取两个经纬度之间的距离
        /// </summary>
        /// <param name="lat1">坐标1的纬度</param>
        /// <param name="lng1">坐标1的经度</param>
        /// <param name="lat2">坐标2的纬度</param>
        /// <param name="lng2">坐标2的经度</param>
        /// <returns>距离</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lng1) - Rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EarthRadius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        public static double GetDistance(decimal lat1, decimal lng1, decimal lat2, decimal lng2)
        {
            return GetDistance((double) lat1, (double) lng1, (double) lat2, (double) lng2);
        }

        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }
    }
}
