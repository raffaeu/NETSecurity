namespace NETAuthentication.IdentityApi.Messages
{
    public class DeleteLoginMessage
    {
        public string Username { get; set; }
        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }
    }

    public class DeleteClaimMessage
    {
        public string Username { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}