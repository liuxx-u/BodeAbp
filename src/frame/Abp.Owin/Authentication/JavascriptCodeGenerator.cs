using System.Collections.Generic;
using Abp.Extensions;

namespace Abp.Owin.Authentication
{
    public class JavascriptCodeGenerator
    {
        /// <summary>
        /// 执行通知的Javascript方法
        /// </summary>
        public string NotifyFuncName => "sso.notify";

        /// <summary>
        /// 执行错误提示的Javascript方法
        /// </summary>
        public string ErrorFuncName => "sso.error";

        public string GetLoginCode(string token, List<string> notifyUrls, string redirectUrl)
        {
            notifyUrls.Insert(0, redirectUrl);

            //第一个元素是登陆成功后跳转的地址，不加token参数
            for (int i = 1; i < notifyUrls.Count; i++)
            {
                notifyUrls[i] = $"{notifyUrls[i]}?token={token}";
            }
            var strUrls = notifyUrls.ExpandAndToString("','");
            return $"{NotifyFuncName}('{strUrls}');";
        }

        public string GetLogoutCode(List<string> notifyUrls)
        {
            notifyUrls.Insert(0, "refresh");
            var strUrls = notifyUrls.ExpandAndToString("','");
            return $"{NotifyFuncName}('{strUrls}');";
        }

        public string GetErrorCode(int code,string message)
        {
            return $"sso.error({code},'{message}')";
        }
    }
}