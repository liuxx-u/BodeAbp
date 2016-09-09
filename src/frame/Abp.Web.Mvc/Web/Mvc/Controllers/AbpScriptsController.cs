﻿using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Extensions;
using Abp.Web.Authorization;
using Abp.Web.Localization;
using Abp.Web.Sessions;
using Abp.Web.Settings;
using Abp.Web.Timing;

namespace Abp.Web.Mvc.Controllers
{
    /// <summary>
    /// This controller is used to create client side scripts
    /// to work with ABP.
    /// </summary>
    public class AbpScriptsController : AbpController
    {
        private readonly ISettingScriptManager _settingScriptManager;
        private readonly ILocalizationScriptManager _localizationScriptManager;
        private readonly IAuthorizationScriptManager _authorizationScriptManager;
        private readonly ISessionScriptManager _sessionScriptManager;
        private readonly ITimingScriptManager _timingScriptManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AbpScriptsController(
            ISettingScriptManager settingScriptManager,
            ILocalizationScriptManager localizationScriptManager,
            IAuthorizationScriptManager authorizationScriptManager,
            ISessionScriptManager sessionScriptManager, 
            ITimingScriptManager timingScriptManager)
        {
            _settingScriptManager = settingScriptManager;
            _localizationScriptManager = localizationScriptManager;
            _authorizationScriptManager = authorizationScriptManager;
            _sessionScriptManager = sessionScriptManager;
            _timingScriptManager = timingScriptManager;
        }

        /// <summary>
        /// Gets all needed scripts.
        /// </summary>
        [DisableAuditing]
        public async Task<ActionResult> GetScripts(string culture = "")
        {
            if (!culture.IsNullOrEmpty())
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }

            var sb = new StringBuilder();
            sb.AppendLine(_sessionScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(_localizationScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(await _authorizationScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(await _settingScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(await _timingScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(GetTriggerScript());

            

            return Content(sb.ToString(), "application/x-javascript", Encoding.UTF8);
        }

        private static string GetTriggerScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.event.trigger('abp.dynamicScriptsInitialized');");
            script.Append("})();");

            return script.ToString();
        }
    }
}