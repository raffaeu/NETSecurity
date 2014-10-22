using NETAuthentication.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.Domain.User
{
    /// <summary>
    /// Represents a registered User
    /// </summary>
    public sealed class UserAggregate : IRootAggregate
    {
        protected UserAggregate()
        {
            this.Claims = new HashSet<UserClaim>();
            this.Logins = new HashSet<UserLogin>();
            this.Roles = new HashSet<RoleAggregate>();
        }

        /// <summary>
        /// Represents the Identifier of the User
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The Username of the User
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The Password of the User
        /// </summary>
        public string PasswordHash { get; private set; }

        /// <summary>
        /// The Security stamp for the password
        /// </summary>
        public string SecurityStamp { get; private set; }

        /// <summary>
        /// The Email of the User
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// The available list of Claims
        /// </summary>
        public ICollection<UserClaim> Claims { get; private set; }

        public ICollection<UserLogin> Logins { get; private set; }

        public ICollection<RoleAggregate> Roles { get; private set; }

        /// <summary>
        /// Create a new Aggregate User
        /// </summary>
        /// <param name="username">The Username</param>
        /// <param name="email">The Email address</param>
        /// <param name="passwordHash">The Password Hash</param>
        /// <param name="securityStamp">The Security stamp of the Password</param>
        /// <returns></returns>
        public static UserAggregate Create(
            string username, string email,
            string passwordHash, string securityStamp)
        {
            return UserAggregate.Create(
                Guid.NewGuid(),
                username,
                email,
                passwordHash,
                securityStamp
                );
        }

        /// <summary>
        /// Create a new Aggregate User
        /// </summary>
        /// <param name="id">The Id associated to the aggregate</param>
        /// <param name="username">The Username</param>
        /// <param name="email">The Email address</param>
        /// <param name="passwordHash">The Password Hash</param>
        /// <param name="securityStamp">The Security stamp of the Password</param>
        /// <returns></returns>
        public static UserAggregate Create(
            Guid id,
            string username, string email,
            string passwordHash, string securityStamp)
        {
            return new UserAggregate
            {
                Id = id,
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp
            };
        }

        /// <summary>
        /// Identifies is the User is in a valid state or not
        /// </summary>
        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(this.Username)
                    && !string.IsNullOrEmpty(this.PasswordHash)
                    && !string.IsNullOrEmpty(this.SecurityStamp)
                    && !string.IsNullOrEmpty(this.Email);
            }
        }

        /// <summary>
        /// Add a new Claim to the User Claims collection
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="originalIssuer"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public void AddClaim(string issuer, string originalIssuer, string type, string value, string valueType)
        {
            var claim = UserClaim.Create(this, issuer, originalIssuer, type, value, valueType);
            if (claim.IsValid)
            {
                this.Claims.Add(claim);                
            }
        }

        public void AddLogin(string providerName, string providerKey)
        {
            var login = UserLogin.Create(this, providerName, providerKey);
            if (login.IsValid)
            {
                this.Logins.Add(login);                
            }
        }

        public void RemoveLogin(UserLogin login)
        {
            this.Logins.Remove(login);
        }

        public void RemoveClaim(UserClaim claim)
        {
            this.Claims.Remove(claim);
        }

        internal void AddRoleToUser(RoleAggregate role)
        {
            this.Roles.Add(role);
        }

        internal void RemoveRoleFromUser(RoleAggregate role)
        {
            this.Roles.Remove(role);
        }
    }
}
