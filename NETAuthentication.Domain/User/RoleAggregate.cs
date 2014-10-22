using NETAuthentication.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.Domain.User
{
    /// <summary>
    /// Represents an available Role
    /// </summary>
    public sealed class RoleAggregate : IRootAggregate
    {
        protected RoleAggregate()
        {
            Users = new HashSet<UserAggregate>();
        }

        /// <summary>
        /// Represents the Identifier of the Role
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The Name of the Role
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A Collection of Users available in this Role
        /// </summary>
        public ICollection<UserAggregate> Users { get; private set; }

        /// <summary>
        /// Identify if the Role is in a valid state or not
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Id != Guid.Empty
                       && !string.IsNullOrEmpty(Name);
            }
        }

        /// <summary>
        /// Create a new Aggregate Role
        /// </summary>
        /// <param name="id">The Id associated to the Aggregate</param>
        /// <param name="name">The Name of the Role</param>
        /// <returns></returns>
        public static RoleAggregate Create(Guid id, string name)
        {
            return new RoleAggregate
            {
                Id = id,
                Name = name
            };
        }

        /// <summary>
        /// Add a User to a Role
        /// </summary>
        /// <param name="user"></param>
        public void AddUserToRole(UserAggregate user)
        {
            if (user.IsValid)
            {
                this.Users.Add(user);
                user.AddRoleToUser(this);
            }
        }

        /// <summary>
        /// Remove a User from a Role
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUserFromRole(UserAggregate user)
        {
            this.Users.Remove(user);
            user.RemoveRoleFromUser(this);
        }
    }
}
