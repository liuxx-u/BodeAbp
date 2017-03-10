using Abp.UI;
using Abp.Web.Mvc.Controllers;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class AdminControllerBase : AbpController
    {
        protected AdminControllerBase()
        {
            LocalizationSourceName = WebDemoConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }
    }
}