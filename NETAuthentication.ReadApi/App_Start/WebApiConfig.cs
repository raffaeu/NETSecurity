using System.Web.Http;

namespace NETAuthentication.ReadApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            // Web API configuration and services
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}