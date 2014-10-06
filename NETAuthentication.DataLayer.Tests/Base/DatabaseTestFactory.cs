using NETAuthentication.DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.DataLayer.Tests.Base
{
    public sealed class DatabaseTestFactory
    {
        public static DatabaseContext GetSession()
        {
            var databaseContext = new DatabaseContext();
            databaseContext.Database.Log = (log) => Debug.WriteLine(log);
            return databaseContext;
        }
    }
}
