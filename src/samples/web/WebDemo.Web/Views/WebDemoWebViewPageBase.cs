using Abp.Web.Mvc.Views;

namespace WebDemo.Web.Views
{
    public abstract class WebDemoWebViewPageBase : WebDemoWebViewPageBase<dynamic>
    {

    }

    public abstract class WebDemoWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected WebDemoWebViewPageBase()
        {
            LocalizationSourceName = WebDemoConsts.LocalizationSourceName;
        }
    }
}