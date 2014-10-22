using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.ReadApi.Tests.Base;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentAssertions;
using System.Net;
using System.Diagnostics;
using NETAuthentication.Domain.Party;
using Newtonsoft.Json;
using NETAuthentication.ReadApi.Messages;
using NETAuthentication.Domain.User;

namespace NETAuthentication.ReadApi.Tests.Controllers
{
    [TestClass]
    public class UserController_Read_Fixtures
    {
        private string iisAddress = "http://localhost:9999/";

        [TestInitialize]
        public void Initialize()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Database.Delete();
                databaseContext.Database.Create();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                databaseContext.Database.Delete();
                databaseContext.Database.Create();
            }
        }

        [TestMethod]
        public void UserController_Get_ReturnOK()
        {
            using (WebApp.Start<Startup>(url: iisAddress))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(String.Format("{0}UserRead", iisAddress)).Result;
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        public void UserController_Get_ReturnUsers()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user01 = UserAggregate.Create("jsmith", "jsmith@acme.com", "password hash", "security stamp");
                var user02 = UserAggregate.Create("mjane", "mjane@acme.com", "password hash", "security stamp");

                databaseContext.Set<UserAggregate>().AddRange(new[] {user01, user02});
                databaseContext.SaveChanges();
            }

            using (WebApp.Start<Startup>(url: iisAddress))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(String.Format("{0}UserRead", iisAddress)).Result;
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var result = response.Content.ReadAsStringAsync().Result;

                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);

                var users = JsonConvert.DeserializeObject<UserMessage[]>(result);

                users.Should()
                    .ContainSingle(x => x.Username == "jsmith");
                users.Should()
                    .ContainSingle(x => x.Username == "mjane");
            }
        }

        [TestMethod]
        public void UserController_GetByUsername_ReturnUser()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var user01 = UserAggregate.Create("jsmith", "jsmith@acme.com", "password hash", "security stamp");
                var user02 = UserAggregate.Create("mjane", "mjane@acme.com", "password hash", "security stamp");

                databaseContext.Set<UserAggregate>().AddRange(new[] { user01, user02 });
                databaseContext.SaveChanges();
            }

            using (WebApp.Start<Startup>(url: iisAddress))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(String.Format("{0}UserRead?username={1}", iisAddress, "jsmith")).Result;
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var result = response.Content.ReadAsStringAsync().Result;

                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);

                var user = JsonConvert.DeserializeObject<UserMessage>(result);

                user.Username.Should()
                    .Be("jsmith");
            }
        }
    }
}
