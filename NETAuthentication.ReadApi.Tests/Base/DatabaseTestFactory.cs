using System.Diagnostics;
using NETAuthentication.DataLayer.Base;

namespace NETAuthentication.ReadApi.Tests.Base
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
