using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.SecurityLayer.Storages
{
    public sealed class CustomUser : IUser<string>
    {
        public string Id { get; private set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public static CustomUser Create(string id, string username, string email, string passwordHash, string securityStamp)
        {
            return new CustomUser
            {
                Id = id,
                UserName = username,
                Email = email,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp
            };
        }
    }
}
