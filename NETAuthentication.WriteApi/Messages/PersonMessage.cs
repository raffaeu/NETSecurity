using System.Runtime.Serialization;

namespace NETAuthentication.WriteApi.Messages
{
    [DataContract]
    public class PersonMessage
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }


}