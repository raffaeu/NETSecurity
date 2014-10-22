using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NETAuthentication.IdentityApi.Messages
{
    public class RegisterUserMessage
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(Username)
                    && !string.IsNullOrEmpty(Password)
                    && !string.IsNullOrEmpty(Email);
            }
        }
    }
}