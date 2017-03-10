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
                        "~/Content/js/jquery.blockUI.js",
                        "~/Content/js/bootstrap.min.js",
                        "~/Content/js/skins.min.js",
                        "~/Content/js/plugs/slimscroll/jquery.slimscroll.min.js",
                        "~/Content/js/beyond.min.js",
                        "~/Content/js/toastr.min.js",
                        "~/Content/js/plugs/sweetalert/sweet-alert.min.js",
                        "~/Content/js/others/spinjs/spin.js",
                        "~/Content/js/others/spinjs/jquery.spin.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",

                        "~/Content/js/md5.min.js",
                        "~/Content/js/store.min.js",
                        "~/Content/js/bode/bode.js",
                        "~/Content/js/bode/bode.ajax.js",
                        "~/Content/js/vue.js"));

            bundles.Add(new StyleBundle("~/AdminStyle").Include(
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/beyond/beyond.min.css",
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/toastr.min.css",
                        "~/Content/js/plugs/sweetalert/sweet-alert.css",
                        "~/Content/css/animate.min.css"));
        }
    }
}