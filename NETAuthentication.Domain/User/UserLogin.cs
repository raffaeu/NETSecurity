using NETAuthentication.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.Domain.User
{
    public sealed class UserLogin : IEntity
    {
        protected UserLogin()
        {
            
        }

        public Guid Id { get; private set; }

        public string ProviderName { get; private set; }

        public string ProviderKey { get; private set; }

        public UserAggregate User { get; private set; }

        public static UserLogin Create(UserAggregate user, string providerName, string providerKey)
        {
            return new UserLogin
            {
                Id = Guid.NewGuid(),
                User = user,
                ProviderKey = providerKey,
                ProviderName = providerName
            };
        }

        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(ProviderName)
                    && !string.IsNullOrEmpty(ProviderKey);
            }
        }
    }
}
