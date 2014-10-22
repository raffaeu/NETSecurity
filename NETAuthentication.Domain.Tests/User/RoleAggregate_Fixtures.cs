using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.Domain.User;
using System.Linq;
using FluentAssertions;
using NETAuthentication.Domain.Base;

namespace NETAuthentication.Domain.Tests.User
{
    [TestClass]
    public class RoleAggregate_Fixtures
    {
        /// <summary>
        /// The aggregate can be constructed only with Factory method
        /// </summary>
        [TestMethod]
        public void RoleAggregate_DontExpose_Constructors()
        {
            var type = typeof (RoleAggregate);
            var ctors = type.GetConstructors();
            ctors.Any(x => x.IsPublic).Should().BeFalse();
        }

        /// <summary>
        /// The aggregate cannot be inherited
        /// </summary>
        [TestMethod]
        public void RoleAggregate_IsSealed()
        {
            var type = typeof (RoleAggregate);
            type.IsSealed.Should().BeTrue();
        }

        /// <summary>
        /// The Role class is an aggregate
        /// </summary>
        [TestMethod]
        public void RoleAggregate_Is_AnAggregate()
        {
            var type = typeof (RoleAggregate);
            typeof(IRootAggregate).IsAssignableFrom(type);
        }

        /// <summary>
        /// The Role created with empty values should not be valid
        /// </summary>
        [TestMethod]
        public void RoleAggregate_Created_EmptyValues_IsNotValid()
        {
            var role = RoleAggregate.Create(Guid.Empty, "");
            role.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void RoleAggregate_Created_ValidValues_IsValid()
        {
            var role = RoleAggregate.Create(Guid.NewGuid(), "name");
            role.IsValid.Should().BeTrue();
        }
    }
}
