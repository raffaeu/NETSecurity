using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.Domain.User;
using NETAuthentication.ReadApi.Messages;

namespace NETAuthentication.ReadApi.Controllers
{
    public class UserReadController : ApiController
    {
        /// <summary>
        ///     Returns a List of available Users
        /// </summary>
        /// <returns>An IEnumerable collection of UserMessage</returns>
        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [HttpGet]
        [Authorize]
        public IEnumerable<UserMessage> Get()
        {
            using (var databaseContext = new DatabaseContext())
            {
                List<UserMessage> messages = databaseContext.Set<UserAggregate>()
                    .Include(x => x.Logins)
                    .Include(x => x.Claims)
                    .ToList()
                    .Select(user => new UserMessage
                    {
                        Id = user.Id,
                        Username = user.Username,
                        PasswordHash = user.PasswordHash,
                        Email = user.Email,
                        SecurityStamp = user.SecurityStamp,
                        Logins = user.Logins.Select(l =>
                            new UserLoginMessage
                            {
                                Provider = l.ProviderName,
                                Key = l.ProviderKey
                            }).ToArray(),
                        Claims = user.Claims.Select(c =>
                            new UserClaimMessage
                            {
                                Issuer = c.Issuer,
                                OriginalIssuer = c.OriginalIssuer,
                                Type = c.Type,
                                Value = c.Value
                            }).ToArray()
                    }).ToList();
                return messages;
            }
        }

        /// <summary>
        ///     Get the current User by Username
        /// </summary>
        /// <param name="username">The Username of the User</param>
        /// <returns>Return an Instance of a User or an empty JSON</returns>
        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [HttpGet]
        [Authorize]
        public UserMessage Get(string username)
        {
            using (var databaseContext = new DatabaseContext())
            {
                UserMessage message = databaseContext.Set<UserAggregate>()
                    .Include(x => x.Logins)
                    .Include(x => x.Claims)
                    .Where(x => x.Username == username)
                    .ToList()
                    .Select(user => new UserMessage
                    {
                        Id = user.Id,
                        Username = user.Username,
                        PasswordHash = user.PasswordHash,
                        Email = user.Email,
                        SecurityStamp = user.SecurityStamp,
                        Logins = user.Logins.Select(l =>
                            new UserLoginMessage
                            {
                                Provider = l.ProviderName,
                                Key = l.ProviderKey
                            }).ToArray(),
                        Claims = user.Claims.Select(c =>
                            new UserClaimMessage
                            {
                                Issuer = c.Issuer,
                                OriginalIssuer = c.OriginalIssuer,
                                Type = c.Type,
                                Value = c.Value
                            }).ToArray()
                    })
                    .SingleOrDefault();
                return message;
            }
        }
    }
}