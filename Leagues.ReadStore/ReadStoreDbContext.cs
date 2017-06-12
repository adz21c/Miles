using System.Data.Entity;

namespace Leagues.ReadStore
{
    public class ReadStoreDbContext : DbContext
    {
        public ReadStoreDbContext() : base("Leagues.ReadStore")
        { }
    }
}
