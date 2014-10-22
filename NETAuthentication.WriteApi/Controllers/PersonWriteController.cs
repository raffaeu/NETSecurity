using System.Web.Http.Cors;
using NETAuthentication.DataLayer.Base;
using NETAuthentication.Domain.Party;
using NETAuthentication.WriteApi.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NETAuthentication.WriteApi.Controllers
{
    public class PersonWriteController : ApiController
    {
        [Authorize]
        [EnableCors(origins: "http://localhost:10001", headers: "*", methods: "*")]
        [HttpPost]
        public HttpResponseMessage Post(PersonMessage person)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var personAggregate = PersonAggregate.Create(
                    person.FirstName, person.LastName);
                databaseContext.Set<PersonAggregate>().Add(personAggregate);

                databaseContext.SaveChanges();

                var response = Request.CreateResponse(HttpStatusCode.Created,personAggregate.Id);
                return response;
            }
        }
    }
}
