using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.Domain.Party;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using NETAuthentication.DataLayer.Tests.Base;

namespace NETAuthentication.DataLayer.Tests.Party
{
    [TestClass]
    public class PersonAggregate_Mapping_Fixtures
    {
        private string connectionString = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Database.Delete();
                databaseContext.Database.Create();
            }
        }

        [TestMethod]
        public void PersonAggregate_Valid_CanBeCreated()
        {
            /// create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var person = PersonAggregate.Create("FirstName", "LastName");
                databaseContext.Set<PersonAggregate>().Add(person);

                databaseContext.SaveChanges();
            }

            /// verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var expectedPerson = databaseContext.Set<PersonAggregate>().First();
                expectedPerson.FirstName.Should().Be("FirstName");
                expectedPerson.LastName.Should().Be("LastName");
            }
        }

        [TestMethod]
        public void PersonAggregate_AddAddress_WillContain()
        {
            /// create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var person = PersonAggregate.Create("John", "Smith");
                person.AddAddress("Street", "City", "Country", "ZipCode");

                databaseContext.Set<PersonAggregate>().Add(person);
                databaseContext.SaveChanges();
            }

            /// verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var person = databaseContext.Set<PersonAggregate>()
                    .Include(x => x.Addresses).First();

                person.Addresses.Count.Should().Be(1);
            }
        }

        [TestMethod]
        public void PersonAggregate_Delete_WithAddresses_WillCascade()
        {
            /// create
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var person = PersonAggregate.Create("John", "Smith");
                person.AddAddress("Street", "City", "Country", "ZipCode");

                databaseContext.Set<PersonAggregate>().Add(person);
                databaseContext.SaveChanges();
            }

            /// delete
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var person = databaseContext.Set<PersonAggregate>().First();

                databaseContext.Set<PersonAggregate>().Remove(person);
                databaseContext.SaveChanges();
            }

            /// verify
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Set<PersonAggregate>().Count().Should().Be(0);
                databaseContext.Set<Address>().Count().Should().Be(0);
            }
        }
    }
}
