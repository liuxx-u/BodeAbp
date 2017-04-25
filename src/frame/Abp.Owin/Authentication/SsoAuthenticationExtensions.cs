using System;
using Microsoft.Owin.Extensions;
using Owin;

namespace Abp.Owin.Authentication
{
    public static class SsoAuthenticationExtensions
    {
        public static IAppBuilder UseSsoCookieAuthentication(this IAppBuilder app, SsoAuthenticationOptions options)
        {
            return app.UseSsoCookieAuthentication(options, PipelineStage.Authenticate);
        }

        public static IAppBuilder UseSsoCookieAuthentication(this IAppBuilder app, SsoAuthenticationOptions options, PipelineStage stage)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            app.Use<SsoAuthenticationMiddleware>(options);
            app.UseStageMarker(stage);
            return app;
        }
    }
}