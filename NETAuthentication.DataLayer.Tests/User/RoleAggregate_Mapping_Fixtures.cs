using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.DataLayer.Tests.Base;
using NETAuthentication.Domain.User;
using System.Data.Entity;

namespace NETAuthentication.DataLayer.Tests.User
{
    [TestClass]
    public class RoleAggregate_Mapping_Fixtures
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
        public void RoleAggregate_Valid_CanBeCreated()
        {
            // create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var role = RoleAggregate.Create(Guid.NewGuid(), "RoleName");
                databaseContext.Set<RoleAggregate>().Add(role);

                databaseContext.SaveChanges();
            }

            // verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var expectedRole = databaseContext.Set<RoleAggregate>().First();
                expectedRole.Name.Should().Be("RoleName");
            }
        }

        [TestMethod]
        public void RoleAggregate_Delete_WillDelete()
        {
            // create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var role = RoleAggregate.Create(Guid.NewGuid(), "RoleName");
                databaseContext.Set<RoleAggregate>().Add(role);

                databaseContext.SaveChanges();
            }

            // delete
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var role = databaseContext.Set<RoleAggregate>().First();
                databaseContext.Set<RoleAggregate>().Remove(role);

                databaseContext.SaveChanges();
            }

            // verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Set<RoleAggregate>().Count().Should().Be(0);
            }
        }

        [TestMethod]
        public void RoleAggregate_AddUser_WillAdd()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var role = RoleAggregate.Create(Guid.NewGuid(), "RoleName");
                var user = UserAggregate.Create("raffaeu", "raffaeu@gmail.com", "password hash", "security stamp");
                role.AddUserToRole(user);

                databaseContext.Set<RoleAggregate>().Add(role);

                databaseContext.SaveChanges();
            }

            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var role = databaseContext.Set<RoleAggregate>()
                    .Include(x => x.Users)
                    .Single();

                role.Users.Count.Should().Be(1);
            }
        }
    }
}
