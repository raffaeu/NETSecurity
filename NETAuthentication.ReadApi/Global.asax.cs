using System.Web;
using System.Web.Http;

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