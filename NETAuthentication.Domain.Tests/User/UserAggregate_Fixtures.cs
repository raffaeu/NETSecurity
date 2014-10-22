using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.Domain.User;
using FluentAssertions;
using System.Linq;
using NETAuthentication.Domain.Base;

namespace NETAuthentication.Domain.Tests.User
{
    [TestClass]
    public class UserAggregate_Fixtures
    {
        /// <summary>
        /// The Aggregate can be constructured only with Factory method
        /// </summary>
        [TestMethod]
        public void UserAggregate_DontExpose_Constructors()
        {
            var type = typeof (UserAggregate);
            var ctors = type.GetConstructors();
            ctors.Any(x => x.IsPublic).Should().BeFalse();
        }

        /// <summary>
        /// The Aggregate cannot be inherited
        /// </summary>
        [TestMethod]
        public void UserAggregate_IsSealed()
        {
            var type = typeof (UserAggregate);
            type.IsSealed.Should().BeTrue();
        }

        /// <summary>
        /// The User class is an aggregate
        /// </summary>
        [TestMethod]
        public void UserAggregate_Is_AnAggregate()
        {
            var type = typeof (UserAggregate);
            typeof (IRootAggregate).IsAssignableFrom(type);
        }

        /// <summary>
        /// The User should have valid properties values
        /// </summary>
        [TestMethod]
        public void UserAggregate_Created_EmptyValues_IsNotValid()
        {
            var user = UserAggregate.Create(
                string.Empty, string.Empty, 
                string.Empty, string.Empty);
            user.IsValid.Should().BeFalse();
        }

        /// <summary>
        /// The User created with valid values should be Valid
        /// </summary>
        [TestMethod]
        public void UserAggregate_Create_ValidValues_IsValid()
        {
            var user = UserAggregate.Create(
                "Username","Email",
                "PasswordHash","SecurityStamp");
            user.IsValid.Should().BeTrue();
        }
    }
}
