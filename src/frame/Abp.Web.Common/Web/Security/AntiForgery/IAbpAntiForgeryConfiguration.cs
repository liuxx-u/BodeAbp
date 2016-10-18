﻿namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// Common configuration shared between ASP.NET Core, ASP.NET MVC and ASP.NET Web API.
    /// </summary>
    public interface IAbpAntiForgeryConfiguration
    {
        /// <summary>
        /// Get/sets cookie name to transfer Anti Forgery token between server and client.
        /// Default value: "XSRF-TOKEN".
        /// </summary>
        string TokenCookieName { get; set; }

        /// <summary>
        /// Get/sets header name to transfer Anti Forgery token from client to the server.
        /// Default value: "X-XSRF-TOKEN". 
        /// </summary>
        string TokenHeaderName { get; set; }
    }
}