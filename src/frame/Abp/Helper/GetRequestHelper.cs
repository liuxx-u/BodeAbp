using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Abp.Helper
{
    /// <summary>
    /// 网页请求枚举
    /// </summary>
    public enum RequestMethod
    {
        Post,
        Get
    }

    public class GetRequestHelper
    {
        /// <summary>
        /// 网页请求
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="requestMethod">请求方法</param>
        /// <param name="param">请求参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="headers">头部</param>
        /// <returns></returns>
        public static string GetWebRequest(string postUrl, RequestMethod requestMethod = RequestMethod.Get ,string param = null, Encoding encoding = null, WebHeaderCollection headers = null)
        {
            postUrl = postUrl.Trim();
            if (postUrl.IndexOf("http://") != 0)
            {
                postUrl = "http://" + postUrl;
            }
            string ret = string.Empty;
            HttpWebResponse response;
            StackTrace st = new StackTrace(true);
            try
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(postUrl));
                if (headers != null) webRequest.Headers = headers;
                webRequest.Method = requestMethod.ToString();
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.UserAgent = "User-Agent";
                webRequest.KeepAlive = true;
                if (param != null && requestMethod == RequestMethod.Post)
                {
                    byte[] paramByte = encoding.GetBytes(param);
                    webRequest.ContentLength = paramByte.Length;
                    var webStream = webRequest.GetRequestStream();
                    webStream.Write(paramByte, 0, paramByte.Length);
                    webStream.Close();
                }
                response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), encoding);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }
    }
}
