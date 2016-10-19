using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebDemo.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/AdminScript").Include(
                        "~/Content/js/jquery.min.js",
                        "~/Content/js/bootstrap.min.js",
                        "~/Content/js/skins.min.js",
                        "~/Content/js/plugs/slimscroll/jquery.slimscroll.min.js",
                        "~/Content/js/beyond.min.js",
                        "~/Content/js/plugs/layer/layer.js",
                        "~/Content/js/md5.min.js",
                        "~/Content/js/bode/bode.js",
                        "~/Content/js/bode/bode.ajax.js",
                        "~/Content/js/vue.js"));

            bundles.Add(new StyleBundle("~/AdminStyle").Include(
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/beyond/beyond.min.css",
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/animate.min.css"));
        }
    }
}