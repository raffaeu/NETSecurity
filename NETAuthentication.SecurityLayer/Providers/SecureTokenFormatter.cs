using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.SecurityLayer.Providers
{
    public class SecureTokenFormatter : ISecureDataFormat<AuthenticationTicket>
    {
        #region Fields

        private TicketSerializer serializer;
        private IDataProtector protector;
        private ITextEncoder encoder;

        #endregion Fields

        #region Constructors

        public SecureTokenFormatter(string key)
        {
            this.serializer = new TicketSerializer();
            this.protector = new OAuthDataProtector(key);
            this.encoder = TextEncodings.Base64Url;
        }

        #endregion Constructors

        #region ISecureDataFormat<AuthenticationTicket> Members

        public string Protect(AuthenticationTicket ticket)
        {
            var ticketData = this.serializer.Serialize(ticket);
            var protectedData = this.protector.Protect(ticketData);
            var protectedString = this.encoder.Encode(protectedData);
            return protectedString;
        }

        public AuthenticationTicket Unprotect(string text)
        {
            var protectedData = this.encoder.Decode(text);
            var ticketData = this.protector.Unprotect(protectedData);
            var ticket = this.serializer.Deserialize(ticketData);
            return ticket;
        }

        #endregion ISecureDataFormat<AuthenticationTicket> Members
    }
}
