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

namespace NETAuthentication.IdentityApi
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
            // store external signIn
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
			
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
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true,
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseFacebookAuthentication(
                appId: "1489718307966120",
                appSecret: "2ddecc9962e5f46a9190fffb8c5b61c0");

            app.UseTwitterAuthentication(
                consumerKey: "8BIodMM86EUDofWibugak3yQ6",
                consumerSecret: "sGilc0PIC4YMA1SBjGaWk65FGYR9sWktqKYq04h5avM3nPsIkk");
        }

	}
}