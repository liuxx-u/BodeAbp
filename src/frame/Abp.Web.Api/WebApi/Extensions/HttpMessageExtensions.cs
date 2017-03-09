using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;


namespace Abp.WebApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpMessageExtensions
    {
        private const string HttpContextKey = "MS_HttpContext";
        private const string OwinContextKey = "MS_OwinContext";

        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        /// <summary>
        /// 返回请求<see cref="HttpRequestMessage"/>是否来自本地
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsLocal(this HttpRequestMessage request)
        {
            var localFlag = request.Properties["MS_IsLocal"] as Lazy<bool>;
            return localFlag != null && localFlag.Value;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContextKey))
            {
                dynamic ctx = request.Properties[HttpContextKey];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }
            if (request.Properties.ContainsKey(OwinContextKey))
            {
                dynamic ctx = request.Properties[OwinContextKey];
                if (ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return null;
        }

        /// <summary>
        /// 将<see cref="HttpResponseMessage"/>使用<see cref="Task"/>来包装
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> ToTask(this HttpResponseMessage responseMessage)
        {
            TaskCompletionSource<HttpResponseMessage> taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(responseMessage);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// 获取<see cref="HttpResponseMessage"/>中包装的错误信息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetErrorMessage(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string msg = "请求处理失败";
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        msg = "请求的资源不存在";
                        break;
                    case HttpStatusCode.BadRequest:
                        msg = "请求中止";
                        break;
                    case HttpStatusCode.Forbidden:
                        msg = "请求被拒绝";
                        break;
                    case HttpStatusCode.ServiceUnavailable:
                        msg = "服务器忙或停机维护";
                        break;
                }
                MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                if (contentType == null || contentType.MediaType != "text/html")
                {
                    HttpError error = response.Content.ReadAsAsync<HttpError>().Result;
                    if (error != null)
                    {
                        string errorMsg = error.Message;
                        if (errorMsg.Contains("An error has occurred"))
                        {
                            errorMsg = error.ExceptionMessage;
                        }
                        msg = string.Format("{0}，详情：{1}", msg, errorMsg);
                    }
                }
                return msg;
            }
            return null;
        }
    }
}