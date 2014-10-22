using Microsoft.AspNet.Identity;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.Domain.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NETAuthentication.SecurityLayer.Storages
{
    public sealed class CustomUsersStorage : 
        IUserStore<CustomUser>, 
        IUserStore<CustomUser, string>,
        IUserPasswordStore<CustomUser>,
        IUserClaimStore<CustomUser>,
        IUserLoginStore<CustomUser>,
        IRoleStore<CustomRole>
    {
        private readonly DatabaseContext databaseContext;

        public CustomUsersStorage()
        {
            this.databaseContext = new DatabaseContext();
        }

        #region IUserStore

        /// <summary>
        /// Insert a new user
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public Task CreateAsync(CustomUser user)
        {
            // verify User does not exists
            var exists = databaseContext.Set<UserAggregate>()
                .Any(x => x.Username == user.UserName
                          || x.Email == user.Email);
            if (exists)
            {
                throw new ApplicationException(String.Format("There is already a User with these Credentials."));
            }

            // create User and Save
            var aggregateUser = UserAggregate.Create(
                id: new Guid(user.Id),
                username: user.UserName,
                email: user.Email,
                passwordHash: user.PasswordHash,
                securityStamp: user.SecurityStamp);
            databaseContext.Set<UserAggregate>().Add(aggregateUser);

            return databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public Task DeleteAsync(CustomUser user)
        {
            var userAggregate = databaseContext
                .Set<UserAggregate>()
                .Where(x => x.Id.ToString() == user.Id)
                .Single();
            databaseContext.Set<UserAggregate>().Remove(userAggregate);

            return databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Finds a user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<CustomUser> IUserStore<CustomUser, string>.FindByIdAsync(string userId)
        {
            var user = databaseContext.Set<UserAggregate>()
                .Single(x => x.Id.ToString() == userId);
            var customUser = CustomUser.Create(
                id: user.Id.ToString(),
                username: user.Username,
                email: user.Email,
                passwordHash: user.PasswordHash,
                securityStamp: user.SecurityStamp);

            return Task.Run(() => { return customUser; });
        }

        /// <summary>
        /// Find a user by name
        /// </summary>
        /// <param name="userName"/>
        /// <returns/>
        Task<CustomUser> IUserStore<CustomUser, string>.FindByNameAsync(string userName)
        {
            var user = databaseContext.Set<UserAggregate>()
                .FirstOrDefault(x => x.Username == userName);
            if (user == null)
            {
                return Task.FromResult<CustomUser>(null);
            }
            var customUser = CustomUser.Create(
                id: user.Id.ToString(),
                username: user.Username,
                email: user.Email,
                passwordHash: user.PasswordHash,
                securityStamp: user.SecurityStamp);

            return Task.FromResult(customUser);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public Task UpdateAsync(CustomUser user)
        {
            var userAggregate = databaseContext
                .Set<UserAggregate>()
                .SingleOrDefault(x => x.Id.ToString() == user.Id);

            // update logic

            return databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            ((DbContext)this.databaseContext).Dispose();
        }

        #endregion

        #region IPasswordStore

        /// <summary>
        /// Get the user password hash
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public Task<string> GetPasswordHashAsync(CustomUser user)
        {
            return Task.FromResult<string>(user.PasswordHash);
        }

        /// <summary>
        /// Returns true if a user has a password set
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public Task<bool> HasPasswordAsync(CustomUser user)
        {
            return Task.FromResult<bool>(user.PasswordHash != null);
        }

        /// <summary>
        /// Set the user password hash
        /// </summary>
        /// <param name="user"/><param name="passwordHash"/>
        /// <returns/>
        public Task SetPasswordHashAsync(CustomUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<int>(0);
        }

        #endregion

        #region IUserClaimStore

        /// <summary>
        /// Add a new user claim
        /// </summary>
        /// <param name="user"/><param name="claim"/>
        /// <returns/>
        public Task AddClaimAsync(CustomUser user, Claim claim)
        {
            var aggregateUser = databaseContext.Set<UserAggregate>()
                .FirstOrDefault(x => x.Id.ToString() == user.Id);
            aggregateUser.AddClaim(claim.Issuer, claim.OriginalIssuer, claim.Type, claim.Value, claim.ValueType);

            return databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieve all the available Claims for a CustomUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(CustomUser user)
        {
            IList<Claim> claims = databaseContext
                .Set<UserClaim>()
                .Where(x => x.User.Id.ToString() == user.Id)
                .ToList()
                .Select(c => new Claim(c.Type, c.Value, c.ValueType, c.Issuer, c.OriginalIssuer))
                .ToList();
            return Task.FromResult(claims);
        }

        public Task RemoveClaimAsync(CustomUser user, Claim claim)
        {
            var userAggregate = databaseContext.Set<UserAggregate>()
                .Include(x => x.Claims)
                .Single(x => x.Id.ToString() == user.Id);
            var userClaim = userAggregate.Claims
                .Single(x => x.Value == claim.Value && x.Type == claim.Type);
            userAggregate.RemoveClaim(userClaim);

            databaseContext.Entry(userClaim).State = EntityState.Deleted;
            return databaseContext.SaveChangesAsync();
        }

        #endregion

        #region ILoginStore

        /// <summary>
        /// Adds a user login with the specified provider and key
        /// </summary>
        /// <param name="user"/><param name="login"/>
        /// <returns/>
        public Task AddLoginAsync(CustomUser user, UserLoginInfo login)
        {
            var aggregateUser = databaseContext.Set<UserAggregate>()
                .FirstOrDefault(x => x.Id.ToString() == user.Id);
            aggregateUser.AddLogin(login.LoginProvider, login.ProviderKey);

            return databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Returns the user associated with this login
        /// </summary>
        /// <returns/>
        public Task<CustomUser> FindAsync(UserLoginInfo login)
        {
            // search a user with this login information
            var aggregateUser = databaseContext.Set<UserAggregate>()
                .Where(
                    x => x.Logins.Any(l => l.ProviderName == login.LoginProvider && l.ProviderKey == login.ProviderKey))
                .SingleOrDefault();

            // if user does not exist, return null
            if (aggregateUser == null)
            {
                return Task.FromResult<CustomUser>(null);
            }

            // else transform the aggregate and return a login info
            return
                Task.FromResult(CustomUser.Create(
                aggregateUser.Id.ToString(), 
                aggregateUser.Username, 
                aggregateUser.Email,
                aggregateUser.PasswordHash, 
                aggregateUser.SecurityStamp));
        }

        /// <summary>
        /// Returns the linked accounts for this user
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(CustomUser user)
        {
            var logins = databaseContext.Set<UserAggregate>()
                .Include(x => x.Logins)
                .Where(x => x.Id.ToString() == user.Id)
                .SelectMany(x => x.Logins)
                .ToList();

            IList<UserLoginInfo> loginInfos = logins.Select(x => new UserLoginInfo(x.ProviderName, x.ProviderKey)).ToList();
            return Task.FromResult(loginInfos);
        }

        /// <summary>
        /// Removes the user login with the specified combination if it exists
        /// </summary>
        /// <param name="user"/><param name="login"/>
        /// <returns/>
        public Task RemoveLoginAsync(CustomUser user, UserLoginInfo login)
        {
            var userAggregate = databaseContext.Set<UserAggregate>()
                .Include(x => x.Logins)
                .Single(x => x.Id.ToString() == user.Id);
            var userLogin = userAggregate.Logins
                .Single(x => x.ProviderKey == login.ProviderKey && x.ProviderName == login.LoginProvider);
            userAggregate.RemoveLogin(userLogin);

            databaseContext.Entry(userLogin).State = EntityState.Deleted;
            return databaseContext.SaveChangesAsync();
        }

        #endregion

        #region IRoleStore

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role"/>
        /// <returns/>
        public Task CreateAsync(CustomRole role)
        {
            // verify Role does not exists
            var exists = databaseContext.Set<RoleAggregate>()
                .Any(x => x.Name == role.Name);
            if (exists)
            {
                throw new ApplicationException(String.Format("There is already a Role with this Name."));
            }

            // create User and Save
            var roleAggregate = RoleAggregate.Create(
                id: new Guid(role.Id),
                name: role.Name);
            databaseContext.Set<RoleAggregate>().Add(roleAggregate);

            return databaseContext.SaveChangesAsync();
        }

        public Task DeleteAsync(CustomRole role)
        {
            var roleAggregate = databaseContext
                .Set<RoleAggregate>()
                .Where(x => x.Id.ToString() == role.Id)
                .Single();
            databaseContext.Set<RoleAggregate>().Remove(roleAggregate);

            return databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Find a role by id
        /// </summary>
        /// <param name="roleId"/>
        /// <returns/>
        Task<CustomRole> IRoleStore<CustomRole, string>.FindByIdAsync(string roleId)
        {
            var role = databaseContext.Set<RoleAggregate>()
                .Single(x => x.Id.ToString() == roleId);
            var customRole = CustomRole.Create(
                id: role.Id.ToString(),
                name: role.Name);

            return Task.Run(() => { return customRole; });
        }

        /// <summary>
        /// Find a role by name
        /// </summary>
        /// <param name="roleName"/>
        /// <returns/>
        Task<CustomRole> IRoleStore<CustomRole, string>.FindByNameAsync(string roleName)
        {
            var role = databaseContext.Set<RoleAggregate>()
                .Single(x => x.Name == roleName);
            var customRole = CustomRole.Create(
                id: role.Id.ToString(),
                name: role.Name);

            return Task.Run(() => { return customRole; });
        }

        public Task UpdateAsync(CustomRole role)
        {
            var userAggregate = databaseContext
                .Set<RoleAggregate>()
                .SingleOrDefault(x => x.Id.ToString() == role.Id);

            // update logic

            return databaseContext.SaveChangesAsync();
        }

        #endregion
    }
}
