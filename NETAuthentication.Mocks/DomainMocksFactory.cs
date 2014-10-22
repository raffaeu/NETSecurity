using NETAuthentication.Domain.Party;
using NETAuthentication.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.Mocks
{
    public class DomainMocksFactory
    {
        public static PersonAggregate Person()
        {
            var person = PersonAggregate.Create("John", "Smith");
            person.AddAddress("Main Street", "New York", "USA", "11100");
            return person;
        }

        public static UserAggregate User()
        {
            var user = UserAggregate.Create("jsmith", "jsmith@acme.com", "password hash", "security stamp");
            return user;
        }
    }
}
