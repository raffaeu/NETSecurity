using NETAuthentication.DataLayer.Base;
using NETAuthentication.Domain.Party;
using NETAuthentication.Domain.User;

namespace NETAuthentication.Mocks
{
    public class DatabaseMocksFactory
    {
        public static void CreateDatabase()
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var person = DomainMocksFactory.Person();
                var user = DomainMocksFactory.User();

                databaseContext.Database.Delete();
                databaseContext.Database.Create();

                databaseContext.Set<PersonAggregate>().Add(person);
                databaseContext.Set<UserAggregate>().Add(user);

                databaseContext.SaveChanges();
            }
        }
    }
}