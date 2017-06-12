using Leagues.ReadStore.Entities;
using System.Data.Entity;
using System.Reflection;

namespace Leagues.ReadStore
{
    public class ReadStoreDbContext : DbContext
    {
        public ReadStoreDbContext() : base("Leagues.ReadStore")
        { }

        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
