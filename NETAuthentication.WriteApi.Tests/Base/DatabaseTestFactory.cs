using NETAuthentication.DataLayer.Base;
using System.Diagnostics;

namespace NETAuthentication.WriteApi.Tests.Base
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
