using MassTransit;
using Miles.MassTransit.MessageDeduplication;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.MessageDeduplication
{
    public class ConsumedRepository : IConsumedRepository
    {
        private readonly DbContext dbContext;

        public ConsumedRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> RecordAsync(MessageContext context, string queueName)
        {
            dbContext.Set<IncomingMessage>().Add(new IncomingMessage(context.MessageId.Value, queueName, DateTime.Now));

            try
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
