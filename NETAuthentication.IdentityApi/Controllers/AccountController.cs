using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using NETAuthentication.IdentityApi.Messages;
using NETAuthentication.SecurityLayer.Storages;
using NETAuthentication.SecurityLayer.Providers;
using System.Threading;

namespace NETAuthentication.IdentityApi.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private readonly UserManager<CustomUser> userManager = new UserManager<CustomUser>(new CustomUsersStorage());

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        /// <summary>
        ///     Register a new User
        /// </summary>
        /// <param name="message">The Information to register the User</param>
        /// <returns>Return the Http status code of the registration</returns>
        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserMessage message)
        {
            await Task.Delay(3000);
            // verify if login credentials are correct
            if (!message.IsValid)
            {
                return BadRequest("The Register User has some invalid data.");
            }

            // if user exists, throw
            var usernameUser = await userManager.FindByNameAsync(message.Username);

            if (usernameUser != null)
            {
                return BadRequest("There is already a User registered with these credentials.");
            }

            // create a new User
            string passwordHash = new PasswordHasher().HashPassword(message.Password);
            CustomUser customUser = CustomUser.Create(Guid.NewGuid().ToString(), message.Username, message.Email,
                passwordHash, DateTime.Now.ToString()
                );

            // register
            await userManager.CreateAsync(customUser);
            return Ok("User has been registered.");
        }

        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [AllowAnonymous]
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [Authorize]
        [HttpPost]
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(DeleteLoginMessage message)
        {
            var user = await userManager.FindByNameAsync(message.Username);
            var logins = await userManager.GetLoginsAsync(user.Id);
            var login = logins.Single(x => x.LoginProvider == message.ProviderName && x.ProviderKey == message.ProviderKey);
            await userManager.RemoveLoginAsync(user.Id, login);

            return Ok();
        }

        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [Authorize]
        [HttpPost]
        [Route("RemoveClaim")]
        public async Task<IHttpActionResult> RemoveClaim(DeleteClaimMessage message)
        {
            var user = await userManager.FindByNameAsync(message.Username);
            var claims = await userManager.GetClaimsAsync(user.Id);
            var claim = claims.Single(x => x.Value == message.Value && x.Type == message.Type);
            await userManager.RemoveClaimAsync(user.Id, claim);

            return Ok();
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            string redirectUriValidationResult = ValidateClientAndRedirectUri(Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            if (error != null)
            {
                redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                    redirectUri,
                    "no",
                    provider,
                    "",
                    "",
                    Uri.EscapeDataString(error),
                    "login");

                return Redirect(redirectUri);
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }


            ExternalLoginInfo info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                    redirectUri,
                    "no",
                    provider,
                    "",
                    info.ExternalIdentity.ToString(),
                    "The Provider refuses to Login!",
                    "login");

                return Redirect(redirectUri);
            }

            // get the claims identity
            ClaimsIdentity claimsIdentity = GetBasicUserIdentity(info);

            // search for a user with this Login authorized
            CustomUser user = await userManager.FindAsync(info.Login);

            if (user == null)
            {
                redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                    redirectUri,
                    "no",
                    provider,
                    info.DefaultUserName,
                    info.ExternalIdentity.ToString(),
                    "There are no User with the login " + info.DefaultUserName + " associated!",
                    "login");

                return Redirect(redirectUri);
            }

            ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

            var hash = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

            redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                redirectUri,
                "yes",
                provider,
                user.UserName,
                hash,
                "",
                "login");

            return Redirect(redirectUri);
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("RegisterExternal", Name = "RegisterExternal")]
        public async Task<IHttpActionResult> GetRegisterExternal(string provider, string username, string error = null)
        {
            string redirectUri = string.Empty;
            // if redirection is wrong don't even continue
            string redirectUriValidationResult = ValidateClientAndRedirectUri(Request, ref redirectUri);
            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            if (error != null)
            {
                redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                    redirectUri,
                    "no",
                    provider,
                    username,
                    "",
                    Uri.EscapeDataString(error),
                    "register");

                return Redirect(redirectUri);
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginInfo info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                    redirectUri,
                    "no",
                    provider,
                    username,
                    "",
                    "The provider refused your Login, please check your Social security settings.",
                    "register");

                return Redirect(redirectUri);
            }

            // get the claims identity
            ClaimsIdentity claimsIdentity = GetBasicUserIdentity(info);

            // verify if user doesn't have already a login associated
            CustomUser loginUser = await userManager.FindAsync(info.Login);

            if (loginUser != null)
            {
                redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                    redirectUri,
                    "no",
                    provider,
                    username,
                    "",
                    string.Format("The User {0} has already an account {1} associated.", username, provider),
                    "register");

                return Redirect(redirectUri);
            }

            // search the User to be registered
            CustomUser user = await userManager.FindByNameAsync(username);

            // register login (from now on he can login with this provider)
            await userManager.AddLoginAsync(user.Id, info.Login);

            // for each claim generated, add the claim
            foreach (var claim in claimsIdentity.Claims)
            {
                await userManager.AddClaimAsync(user.Id, claim);
            }

            redirectUri = string.Format("{0}?&authenticated={1}&provider={2}&username={3}&token={4}&errorMessage={5}&operation={6}",
                redirectUri,
                "yes",
                provider,
                username,
                "",
                "",
                "register");

            return Redirect(redirectUri);
        }

        [Authorize]
        [HttpGet]
        [Route("ProtectedCall")]
        public IHttpActionResult ProtectedCall()
        {
            return Ok("This call has been authenticated");
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            Uri redirectUri;

            string redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            // to do
            // we may check for Client Id

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;
        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            IEnumerable<KeyValuePair<string, string>> queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null)
            {
                return null;
            }

            KeyValuePair<string, string> match =
                queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value))
            {
                return null;
            }

            return match.Value;
        }

        /// <summary>
        /// Build the basic user identity claim
        /// </summary>
        /// <param name="name">The name of the User</param>
        /// <returns></returns>
        protected ClaimsIdentity GetBasicUserIdentity(ExternalLoginInfo info)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, info.DefaultUserName)
            };
            return new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        }

    }
}