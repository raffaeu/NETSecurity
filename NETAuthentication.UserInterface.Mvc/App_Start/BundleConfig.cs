using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace NETAuthentication.UserInterface.Mvc.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-{version}.js")
                    //.Include("~/Scripts/modernizr-2.8.3.js")
            );

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap")
                    .Include("~/Scripts/bootstrap.js")
            );

            bundles.Add(
                new StyleBundle("~/styles/bootstrap")
                    .Include("~/Content/bootstrap.css")
                    .Include("~/Content/bootstrap-icons.css")
                    .Include("~/Content/font-awesome-4.2.0/css/font-awesome.css")
                    .Include("~/Content/Site.css")
            );

            bundles.Add(
                new ScriptBundle("~/bundles/angular")
                    .Include("~/Scripts/Angular/angular.js")
                    .Include("~/Scripts/Angular/angular-route.js")
                    .Include("~/Scripts/Angular/angular-cookies.js")
                    .Include("~/Scripts/Angular/angular-modal-service.js")
                    .Include("~/Scripts/Angular/angular-ngstorage.js")
            );

            bundles.Add(
                new ScriptBundle("~/bundles/app")
                    .Include("~/Scripts/ui-bootstrap-0.11.2.js")
                    .Include("~/Scripts/ui-bootstrap-tpls-0.11.2.js")
                    .Include("~/Scripts/AppStart.js")    
            );
        }
    }
}