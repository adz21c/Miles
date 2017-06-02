using Miles.MassTransit.MessageDispatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Miles.MassTransit.AspNet
{
    public class HostingEnvrionmentMessageDispatchProcess : IMessageDispatchProcess
    {
        private readonly IMessageDispatcher commandDispatcher;
        private readonly ConventionBasedMessageDispatcher eventDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingEnvrionmentMessageDispatchProcess"/> class.
        /// </summary>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public HostingEnvrionmentMessageDispatchProcess(
            IMessageDispatcher commandDispatcher,
            ConventionBasedMessageDispatcher eventDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
            this.eventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// Initiates the dispatch of messages to the message queue
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            HostingEnvironment.QueueBackgroundWorkItem(async cancellationToken =>
            {
                foreach (var message in messages)
                {
                    var dispatcher = message.ConceptType == OutgoingMessageConceptType.Command ? commandDispatcher : eventDispatcher;
                    await dispatcher.DispatchAsync(message).ConfigureAwait(false);
                }
            });

            return Task.CompletedTask;
        }
    }
}
