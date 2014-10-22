using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using NETAuthentication.SecurityLayer.Providers;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NETAuthentication.ReadApi
{
	public partial class Startup
	{
		/// <summary>
		///  The authentication options
		/// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

		/// <summary>
		/// The Id of the Caller
		/// </summary>
        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(YetAnotherTodoDbContext<ApplicationUser>.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
			
            // We're enabling cookie authentication, but with a specific cookie name.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieHttpOnly = false,
                CookieName = "NETAuthentication"
            });

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/Account/Login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true,
                //AuthorizationCodeFormat = new TicketDataFormat(new OAuthDataProtector("key")),
                //RefreshTokenFormat = new TicketDataFormat(new OAuthDataProtector("key")),
                //AccessTokenFormat = new TicketDataFormat(new OAuthDataProtector("key"))
                
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }

	}
}