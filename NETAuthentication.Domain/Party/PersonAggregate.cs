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
    }
}
