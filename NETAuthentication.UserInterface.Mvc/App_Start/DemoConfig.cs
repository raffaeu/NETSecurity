using NETAuthentication.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NETAuthentication.UserInterface.Mvc.App_Start
{
    public class DemoConfig
    {
        public static void RegisterDemo()
        {
            DatabaseMocksFactory.CreateDatabase();
        }
    }
}