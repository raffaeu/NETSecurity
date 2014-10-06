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
                    .Include("~/Scripts/modernizr-2.8.3.js")
            );

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap")
                    .Include("~/Scripts/bootstrap.min.js")
            );

            bundles.Add(
                new StyleBundle("~/bundles/bootstrap")
                    .Include("~/Content/bootstrap.min.css")
                    .Include("~/Content/bootstrap-theme.min.css")
            );

            // Code removed for clarity.
            BundleTable.EnableOptimizations = true;
        }
    }
}