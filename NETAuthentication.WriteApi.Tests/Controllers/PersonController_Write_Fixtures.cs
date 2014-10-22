using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETAuthentication.DataLayer.Base;
using Microsoft.Owin.Hosting;
using NETAuthentication.WriteApi.Tests.Base;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentAssertions;
using System.Net;
using System.Diagnostics;
using NETAuthentication.WriteApi.Messages;

namespace NETAuthentication.WriteApi.Tests
{
    [TestClass]
    public class PersonController_Write_Fixtures
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
        public void PersonController_Post_ReturnOK()
        {
            var personMessage = new PersonMessage
            {
                FirstName = "John",
                LastName = "Smith"
            };

            using (WebApp.Start<Startup>(url: iisAddress))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync(String.Format("{0}PersonWrite", iisAddress),personMessage).Result;
                response.StatusCode.Should().Be(HttpStatusCode.Created);

                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
