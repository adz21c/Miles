﻿using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Sets up the child container/scope such that implementations of Miles interfaces are setup correctly
    /// for MassTransit when resolving the message processor.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="MassTransit.IConsumer{TMessage}" />
    public class Consumer<TMessage> : IConsumer<TMessage> where TMessage : class
    {
        private readonly static Task CompletedTask = Task.FromResult(0);    // Replace with Task.CompletedTask in .NET 4.6

        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Consumer{TMessage}" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Consumer(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Processes the incoming message.
        /// </summary>
        /// <param name="context">The consumer context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            container.RegisterConsumeContext(context);
            var processor = container.ResolveProcessor<TMessage>();
            await processor.ProcessAsync(context.Message);
        }
    }
}