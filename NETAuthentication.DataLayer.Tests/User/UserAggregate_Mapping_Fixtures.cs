using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.DataLayer.Tests.Base;
using NETAuthentication.Domain.User;
using System.Linq;
using FluentAssertions;
using System.Data.Entity;

namespace NETAuthentication.DataLayer.Tests.User
{
    [TestClass]
    public class UserAggregate_Mapping_Fixtures
    {
        [TestInitialize]
        public void Initialize()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Database.Delete();
                databaseContext.Database.Create();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Database.Delete();
                databaseContext.Database.Create();
            }
        }

        [TestMethod]
        public void UserAggregate_Valid_CanBeCreate()
        {
            // create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user = UserAggregate.Create("raffaeu", "raffaeu@gmail.com", 
                    "password hash", "security stamp");
                databaseContext.Set<UserAggregate>().Add(user);

                databaseContext.SaveChanges();
            }

            // verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var expectedUser = databaseContext.Set<UserAggregate>().First();
                expectedUser.Username.Should().Be("raffaeu");
            }
        }

        [TestMethod]
        public void UserAggregate_Delete_WillDelete()
        {
            // create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user = UserAggregate.Create("raffaeu", "raffaeu@gmail.com",
                    "password hash", "security stamp");
                databaseContext.Set<UserAggregate>().Add(user);
                databaseContext.SaveChanges();
            }

            // delete
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user = databaseContext.Set<UserAggregate>().First();
                databaseContext.Set<UserAggregate>().Remove(user);

                databaseContext.SaveChanges();
            }

            // verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Set<UserAggregate>().Count().Should().Be(0);
            }
        }

        [TestMethod]
        public void UserAggregate_AddClaim_WillPersist()
        {
            // create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user = UserAggregate.Create("raffaeu", "raffaeu@gmail.com",
                    "password hash", "security stamp");

                user.AddClaim("LOCAL AUTHORITY", "LOCAL AUTHORITY", "schema", "Raffaeu", "schema#string");
                
                
                databaseContext.Set<UserAggregate>().Add(user);
                databaseContext.SaveChanges();
            }

            // verify
            using (DatabaseContext databaseContext =  DatabaseTestFactory.GetSession())
            {
                var expectedUser = databaseContext.Set<UserAggregate>()
                    .Include(x => x.Claims)
                    .First();

                expectedUser.Claims.Count.Should().Be(1);
            }
        }

        [TestMethod]
        public void UserAggregate_AddLogin_WillPersist()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user = UserAggregate.Create("raffaeu", "raffaeu@gmail.com",
                    "password hash", "security stamp");
                user.AddLogin("providerName", "providerKey");

                databaseContext.Set<UserAggregate>().Add(user);
                databaseContext.SaveChanges();
            }

            // verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var expectedUser = databaseContext.Set<UserAggregate>()
                    .Include(x => x.Logins)
                    .First();

                expectedUser.Logins.Count.Should().Be(1);
            }
        }
    }
}
