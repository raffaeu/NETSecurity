using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace NETAuthentication.IdentityApi.Controllers
{
    public class ChallengeResult : IHttpActionResult
    {
        public string LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }

        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            //Request.GetOwinContext().Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            Request.GetOwinContext().Authentication.Challenge(LoginProvider);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}