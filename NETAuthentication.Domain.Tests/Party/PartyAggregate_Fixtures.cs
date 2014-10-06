using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.Domain.Party;
using NETAuthentication.Domain.Base;

namespace NETAuthentication.Domain.Tests.Party
{
    [TestClass]
    public class PartyAggregate_Fixtures
    {
        /// <summary>
        /// The Aggregate can be constructed only with Factory method
        /// </summary>
        [TestMethod]
        public void PersonAggregate_DontExpose_Constructors()
        {
            var type = typeof (PersonAggregate);
            var ctors = type.GetConstructors();
            ctors.Any(x => x.IsPublic).Should().BeFalse();
        }

        /// <summary>
        /// The Aggregate cannot be inherited
        /// </summary>
        [TestMethod]
        public void PersonAggregate_IsSealed()
        {
            var type = typeof (PersonAggregate);
            type.IsSealed.Should().BeTrue();
        }

        /// <summary>
        /// The Person class is an aggregate
        /// </summary>
        [TestMethod]
        public void PersonAggregate_Is_AnAggregate()
        {
            var type = typeof (PersonAggregate);
            typeof (IRootAggregate).IsAssignableFrom(type);
        }

        /// <summary>
        /// The Person should have valid FirstName and LastName
        /// </summary>
        [TestMethod]
        public void PersonAggregate_Created_EmptyValues_IsNotValid()
        {
            var person = PersonAggregate.Create(string.Empty, string.Empty);
            person.IsValid.Should().BeFalse();
        }

        /// <summary>
        /// The Person should have valid Firstname and LastName
        /// </summary>
        [TestMethod]
        public void PersonAggregate_Create_ValidValues_IsValid()
        {
            var person = PersonAggregate.Create("John", "Smith");
            person.IsValid.Should().BeTrue();
        }

        /// <summary>
        /// A Person initialized has an empty collection of addresses
        /// </summary>
        [TestMethod]
        public void PersonAggregate_Create_HasEmptyAddress()
        {
            var person = PersonAggregate.Create("John", "Smith");
            person.Addresses.Should().NotBeNull();
            person.Addresses.Should().BeEmpty();
        }

        /// <summary>
        /// If the Address is not valid the Person cannot add it
        /// </summary>
        [TestMethod]
        public void PersonAggregate_Add_InvalidAddress_Fail()
        {
            var person = PersonAggregate.Create("John", "Smith");
            person.AddAddress(string.Empty, string.Empty, string.Empty, string.Empty);

            person.Addresses.Should().BeEmpty();
        }

        /// <summary>
        /// If the Address is valid the Person should contain it
        /// </summary>
        [TestMethod]
        public void PersonAggregate_Add_ValidAddress_WillContain()
        {
            var person = PersonAggregate.Create("John", "Smith");
            person.AddAddress("Street", "City", "Country", "ZipCode");

            person.Addresses.Should().NotBeEmpty();
            person.Addresses.Count.Should().Be(1);
        }
    }
}
