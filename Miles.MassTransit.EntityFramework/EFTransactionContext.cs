using Miles.Persistence;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework
{
    public class EFTransactionContext : TransactionContextBase
    {
        private readonly DbContext dbContext;
        private DbContextTransaction transaction = null;

        public EFTransactionContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task DoBeginAsync(IsolationLevel? hintIsolationLevel = null)
        {
            if (hintIsolationLevel.HasValue)
                transaction = dbContext.Database.BeginTransaction(hintIsolationLevel.Value);
            else
                transaction = dbContext.Database.BeginTransaction();

            return Task.CompletedTask;
        }

        protected override Task DoCommitAsync()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }

            return Task.CompletedTask;
        }

        protected override Task DoRollbackAsync()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }

            return Task.CompletedTask;
        }
    }
}
