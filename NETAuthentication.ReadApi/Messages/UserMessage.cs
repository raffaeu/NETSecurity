using System;
using System.Runtime.Serialization;

namespace NETAuthentication.ReadApi.Messages
{
    [DataContract]
    public class UserMessage
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public string SecurityStamp { get; set; }

        [DataMember]
        public UserLoginMessage[] Logins { get; set; }
        [DataMember]
        public UserClaimMessage[] Claims { get; set; }
    }

    [DataContract]
    public class UserLoginMessage
    {
        [DataMember]
        public string Provider { get; set; }
        [DataMember]
        public string Key { get; set; }
    }

    [DataContract]
    public class UserClaimMessage
    {
        [DataMember]
        public string Issuer { get; set; }
        [DataMember]
        public string OriginalIssuer { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}