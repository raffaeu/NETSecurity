﻿using NETAuthentication.UserInterface.Mvc.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace NETAuthentication.UserInterface.Mvc
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DemoConfig.RegisterDemo();
        } 
    }
}