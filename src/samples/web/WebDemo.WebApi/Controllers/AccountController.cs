using System;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.UI;
using Abp.Web.Models;
using Abp.WebApi.Controllers;
using WebDemo.Api.Models;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using BodeAbp.Zero.Users.Domain;
using Abp.Domain.Uow;
using Abp.Authorization.Users.Domain;

namespace WebDemo.Api.Controllers
{
    public class AccountController : AbpApiController
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        private readonly AbpUserManager _userManager;

        static AccountController()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }

        public AccountController(AbpUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [UnitOfWork]
        public async Task<AjaxResponse> Authenticate(LoginModel loginModel)
        {
            CheckModelState();
            var loginResult = await _userManager.LoginAsync(loginModel.UserName, loginModel.Password);
            if (loginResult.Result == AbpLoginResultType.Success)
            {

                var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

                var currentUtc = new SystemClock().UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(30));
                return new AjaxResponse(OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
            }
            else
            {
                switch (loginResult.Result)
                {
                    case AbpLoginResultType.InvalidUserName:
                    case AbpLoginResultType.InvalidPassword:
                        throw new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                    case AbpLoginResultType.UserIsNotActive:
                        throw new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", loginModel.UserName));
                    case AbpLoginResultType.EmailIsNotConfirmed:
                        throw new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                    default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                        Logger.Warn("Unhandled login fail reason: " + loginResult.Result);
                        throw new UserFriendlyException(L("LoginFailed"));
                }
            }
        }
    }
}
