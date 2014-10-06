using NETAuthentication.Domain.Party;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.DataLayer.Mappings
{
    public sealed class PersonAggregateMap : EntityTypeConfiguration<PersonAggregate>
    {
        public PersonAggregateMap()
        {
            this.ToTable("Person");

            this.HasMany(x => x.Addresses)
                .WithRequired(x => x.Person)
                .WillCascadeOnDelete(true);
        }
    }
}
