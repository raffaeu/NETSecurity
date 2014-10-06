using NETAuthentication.DataLayer.Base;
using NETAuthentication.Domain.Party;
using NETAuthentication.ReadApi.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NETAuthentication.ReadApi.Controllers
{
    /// <summary>
    /// Controller in charge of returning instances of a Person
    /// </summary>
    public class PersonController : ApiController
    {
        /// <summary>
        /// Returns a List of available Persons
        /// </summary>
        /// <returns>An IEnumerable collection of PersonMessage</returns>
        [HttpGet]
        public IEnumerable<PersonMessage> Get()
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var messages = databaseContext.Set<PersonAggregate>()
                    .Select(person => new PersonMessage
                    {
                        Id = person.Id,
                        FirstName = person.FirstName,
                        LastName = person.LastName
                    }).ToList();
                return messages;
            }
        }
    }
}
