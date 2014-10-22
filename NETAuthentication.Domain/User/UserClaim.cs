using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NETAuthentication.Domain.Base;

namespace NETAuthentication.Domain.User
{
    public sealed class UserClaim : IEntity
    {
        protected UserClaim()
        {
            
        }

        /// <summary>
        /// The Unique Id of the stored claim
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The name of the Claim Issuer
        /// </summary>
        public string Issuer { get; private set; }

        /// <summary>
        /// The name of the Original Claim Issuer
        /// </summary>
        public string OriginalIssuer { get; private set; }

        /// <summary>
        /// The Type of claim
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The Value of the claim
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The value type of the claim
        /// </summary>
        public string ValueType { get; private set; }

        public UserAggregate User { get; private set; }

        /// <summary>
        /// Generate a new Claim associated to a User
        /// </summary>
        /// <param name="user">The parent User</param>
        /// <param name="issuer">The Claim Issuer</param>
        /// <param name="originalIssuer">The Original Claim Issuer</param>
        /// <param name="type">The Type of the Claim</param>
        /// <param name="value">The Value of the Claim</param>
        /// <param name="valueType">The Value Type of the Claim</param>
        /// <returns></returns>
        internal static UserClaim Create(UserAggregate user, string issuer, string originalIssuer, string type, string value, string valueType)
        {
            return new UserClaim
            {
                Id = Guid.NewGuid(),
                User = user,
                Issuer = issuer,
                OriginalIssuer = originalIssuer,
                Type = type,
                Value = value,
                ValueType = valueType
            };
        }


        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(this.Issuer)
                    && !string.IsNullOrEmpty(this.OriginalIssuer)
                    && !string.IsNullOrEmpty(this.Type)
                    && !string.IsNullOrEmpty(this.ValueType);
            }
        }
    }
}
