using System.Web;
using System.Web.Mvc;

namespace NETAuthentication.WriteApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
