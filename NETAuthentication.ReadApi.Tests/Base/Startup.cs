using NETAuthentication.ReadApi.App_Start;
using Owin;
using System.Web.Http;

namespace NETAuthentication.ReadApi.Tests.Base
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                            name: "Default",
                            routeTemplate: "{controller}/{id}",
                            defaults: new { id = RouteParameter.Optional }
                        );

            WebApiConfig.Register(config);

            appBuilder.UseWebApi(config);
        }
    }
}