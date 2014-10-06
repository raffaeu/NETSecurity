using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.Domain.Party;
using System.Linq;
using FluentAssertions;

namespace NETAuthentication.Domain.Tests.Party
{
    [TestClass]
    public class Address_Fixtures
    {
        /// <summary>
        /// The Address can be constructed only with Factory method
        /// </summary>
        [TestMethod]
        public void Address_DontExpose_Constructors()
        {
            var type = typeof (Address);
            var ctors = type.GetConstructors();
            ctors.Any(x => x.IsPublic).Should().BeFalse();
        }

        /// <summary>
        /// The Address cannot be inherited
        /// </summary>
        [TestMethod]
        public void Address_IsSealed()
        {
            var type = typeof (Address);
            type.IsSealed.Should().BeTrue();
        }

        /// <summary>
        /// An Address without a Person is not valid
        /// </summary>
        [TestMethod]
        public void Address_Create_NullPerson_IsNotValid()
        {
            var address = Address.Create(null, string.Empty, string.Empty, string.Empty, string.Empty);
            address.IsValid.Should().BeFalse();
        }

        /// <summary>
        /// An Address with valid values should be valid
        /// </summary>
        [TestMethod]
        public void Address_Create_ValidValues_IsValid()
        {
            var person = PersonAggregate.Create("John", "Smith");
            var address = Address.Create(person, "Street", "City", "Country", "ZipCode");

            address.IsValid.Should().BeTrue();
        }
    }
}
