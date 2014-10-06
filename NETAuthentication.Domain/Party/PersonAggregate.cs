using NETAuthentication.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.Domain.Party
{
    /// <summary>
    /// Represents a Person root aggregate
    /// </summary>
    public sealed class PersonAggregate : IRootAggregate
    {
        /// <summary>
        /// Protected constructor required by Entity Framework and by Factory pattern
        /// </summary>
        protected PersonAggregate()
        {
            this.Addresses = new HashSet<Address>();
        }

        /// <summary>
        /// Represents the Identifier of the Person
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The FirstName of the Person
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// The LastName of the Person
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// A Collection of Addresses
        /// </summary>
        public ICollection<Address> Addresses { get; private set; }

        /// <summary>
        /// Create a new instance of a Person object
        /// </summary>
        /// <param name="firstName">The value associated to the FirstName</param>
        /// <param name="lastName">The value associated to the LastName</param>
        /// <returns></returns>
        public static PersonAggregate Create(string firstName, string lastName)
        {
            return new PersonAggregate
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName
            };
        }

        /// <summary>
        /// Identifies if the Person is in a valid state or not
        /// </summary>
        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(this.FirstName)
                    && !string.IsNullOrEmpty(this.LastName);
            }
        }

        /// <summary>
        /// Create a new Address and add it to the collection of Addresses
        /// </summary>
        /// <param name="person">The parent Person</param>
        /// <param name="street">The Street of the Address</param>
        /// <param name="city">The City of the Address</param>
        /// <param name="country">The Country of the Address</param>
        /// <param name="zipCode">The ZipCode of the Address</param>
        public void AddAddress(string street, string city, string country, string zipCode)
        {
            var address = Address.Create(this, street, city, country, zipCode);
            if (address.IsValid)
            {
                this.Addresses.Add(address);
            }
        }
    }
}
