using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.MessageDispatch;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.MessageDeduplication
{
    public class OutgoingMessageRepository : IOutgoingMessageRepository
    {
        private readonly DbContext dbContext;
        private readonly ITime time;

        public OutgoingMessageRepository(DbContext dbContext, ITime time)
        {
            this.dbContext = dbContext;
            this.time = time;
        }

        public async Task SaveAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            var currentTime = time.Now;
            dbContext.Set<OutgoingMessage>().AddRange(messages.Select(x => new OutgoingMessage(
                    x.MessageId,
                    x.CorrelationId,
                    x.MessageType.FullName,
                    x.ConceptType,
                    JsonConvert.SerializeObject(x.MessageObject),
                    currentTime)));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
