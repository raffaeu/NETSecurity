using System.Data.Entity.ModelConfiguration;
using NETAuthentication.Domain.User;

namespace NETAuthentication.DataLayer.Mappings
{
    public sealed class RoleAggregateMap : EntityTypeConfiguration<RoleAggregate>
    {
        public RoleAggregateMap()
        {
            this.ToTable("Role");

            this.HasMany(x => x.Users)
                .WithMany();
        }
    }
}