using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using NETAuthentication.ReadApi.App_Start;

namespace NETAuthentication.ReadApi
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            // configure routes
            GlobalConfiguration.Configuration.Routes.MapHttpRoute("Default", 
                "{controller}/{id}", new
                {
                    id = RouteParameter.Optional
                });

            // configure web api
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}