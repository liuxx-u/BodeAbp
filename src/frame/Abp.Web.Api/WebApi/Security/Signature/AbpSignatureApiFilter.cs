using Abp.Dependency;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using System.Net;
using Abp.WebApi.Configuration;
using System.Web;
using System.IO;
using Abp.Helper;

namespace Abp.WebApi.Security.Signature
{
    public class AbpSignatureApiFilter : IAuthorizationFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public bool AllowMultiple => false;


        private readonly IAbpWebApiConfiguration _webApiConfiguration;

        public AbpSignatureApiFilter(IAbpWebApiConfiguration webApiConfiguration)
        {
            _webApiConfiguration = webApiConfiguration;
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (!_webApiConfiguration.IsSignatureValidationEnable)
            {
                return await continuation();
            }
            if (!actionContext.Request.Headers.Contains("timestamp") || !actionContext.Request.Headers.Contains("nonce") || !actionContext.Request.Headers.Contains("signature"))
            {
                return CreateErrorResponse(actionContext, "Empty or invalid signature header.");
            }

            var timestamp = actionContext.Request.Headers.GetValues("timestamp").First();
            var nonce = actionContext.Request.Headers.GetValues("nonce").First();
            var signature = actionContext.Request.Headers.GetValues("signature").First();

            var timespan = long.Parse(DateTimeHelper.GetTimeStamp(false)) - long.Parse(timestamp);
            if (Math.Abs(timespan) > 2 * 60 * 60 * 1000)//请求时间与服务器时间差超过两小时
            {
                return CreateErrorResponse(actionContext, "request time error.");
            } 

            string data = string.Empty;
            var method = actionContext.Request.Method;
            if (method == HttpMethod.Post)
            {
                StreamReader streamReader = new StreamReader(HttpContext.Current.Request.InputStream);
                data = streamReader.ReadToEnd();
            }

            var sign = HashHelper.GetMd5(HashHelper.GetMd5(timestamp) + HashHelper.GetMd5(nonce) + HashHelper.GetMd5(data));
            if (sign != signature)
            {
                return CreateErrorResponse(actionContext, "signature verification failed.");
            }

            return await continuation();
        }

        protected virtual HttpResponseMessage CreateErrorResponse(HttpActionContext actionContext, string reason)
        {
            Logger.Warn(reason);
            Logger.Warn("Requested URI: " + actionContext.Request.RequestUri);
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = reason };
        }
    }
}
