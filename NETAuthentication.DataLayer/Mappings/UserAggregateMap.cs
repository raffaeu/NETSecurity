using NETAuthentication.Domain.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAuthentication.DataLayer.Mappings
{
    public sealed class UserAggregateMap : EntityTypeConfiguration<UserAggregate>
    {
        public UserAggregateMap()
        {
            this.ToTable("User");

            this.HasMany(x => x.Claims)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);

            this.HasMany(x => x.Logins)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);
        }
    }
}
