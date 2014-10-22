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

namespace NETAuthentication.ReadApi.Tests.Controllers
{
    [TestClass]
    public class PersonController_Read_Fixtures
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
        public void PersonController_Get_ReturnOK()
        {
            using (WebApp.Start<Startup>(url: iisAddress))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(String.Format("{0}PersonRead", iisAddress)).Result;
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        public void PersonController_Get_ReturnPersons()
        {
            using (DatabaseContext databaseContext = DatabaseTestFactory.GetSession())
            {
                var person01 = PersonAggregate.Create("John", "Smith");
                var person02 = PersonAggregate.Create("Mary", "Jane");

                databaseContext.Set<PersonAggregate>().AddRange(new[] {person01, person02});
                databaseContext.SaveChanges();
            }

            using (WebApp.Start<Startup>(url: iisAddress))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(String.Format("{0}PersonRead", iisAddress)).Result;
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var result = response.Content.ReadAsStringAsync().Result;

                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);

                var persons = JsonConvert.DeserializeObject<PersonMessage[]>(result);

                persons.Should()
                    .ContainSingle(x => x.FirstName == "John" && x.LastName == "Smith");
                persons.Should()
                    .ContainSingle(x => x.FirstName == "Mary" && x.LastName == "Jane");
            }
        }
    }
}
