using NETAuthentication.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.Domain.Party
{
    /// <summary>
    /// Represents an Address entity
    /// </summary>
    public sealed class Address : IEntity
    {
        /// <summary>
        /// Protected constructor required by Entity Framework and by Factory pattern
        /// </summary>
        protected Address()
        {
            
        }

        /// <summary>
        /// Represents the Identifier of the Address
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Represents the Parent Person aggregate
        /// </summary>
        public PersonAggregate Person { get; private set; }

        /// <summary>
        /// Represents the Street of the Address
        /// </summary>
        public string Street { get; private set; }

        /// <summary>
        /// Represents the City of the Address
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// Represents the Country of the Address
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// Represents the ZipCode of the Address
        /// </summary>
        public string ZipCode { get; private set; }

        /// <summary>
        /// Internal factory method to create a new Address
        /// </summary>
        /// <param name="person">The parent Person</param>
        /// <param name="street">The Street of the Address</param>
        /// <param name="city">The City of the Address</param>
        /// <param name="country">The Country of the Address</param>
        /// <param name="zipCode">The ZipCode of the Address</param>
        /// <returns></returns>
        internal static Address Create(PersonAggregate person, string street, string city, string country,
            string zipCode)
        {
            return new Address
            {
                Id = Guid.NewGuid(),
                Person = person,
                Street = street,
                City = city,
                Country = country,
                ZipCode = zipCode
            };
        }

        /// <summary>
        /// Identifies if the Address is in a valid state or not
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Person != null
                       && !string.IsNullOrEmpty(Street)
                       && !string.IsNullOrEmpty(City)
                       && !string.IsNullOrEmpty(Country)
                       && !string.IsNullOrEmpty(ZipCode);
            }
        }
    }
}
