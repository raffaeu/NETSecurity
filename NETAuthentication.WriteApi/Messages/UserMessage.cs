using System;
using System.Runtime.Serialization;

namespace NETAuthentication.WriteApi.Messages
{
    [DataContract]
    public class UserMessage
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

    }
}