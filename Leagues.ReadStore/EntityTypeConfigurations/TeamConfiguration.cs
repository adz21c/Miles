using Leagues.ReadStore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Leagues.ReadStore.EntityTypeConfigurations
{
    public class TeamConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            HasKey(x => x.Id).Property(x => x.Id).HasMaxLength(10).IsRequired();
            Property(x => x.Name).HasMaxLength(128).IsRequired();
        }
    }
}
