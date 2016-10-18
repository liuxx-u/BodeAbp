﻿namespace Abp.Web.Security.AntiForgery
{
    public class AbpAntiForgeryConfiguration : IAbpAntiForgeryConfiguration
    {
        public string TokenCookieName { get; set; }

        public string TokenHeaderName { get; set; }

        public AbpAntiForgeryConfiguration()
        {
            TokenCookieName = "XSRF-TOKEN";
            TokenHeaderName = "X-XSRF-TOKEN";
        }
    }
}