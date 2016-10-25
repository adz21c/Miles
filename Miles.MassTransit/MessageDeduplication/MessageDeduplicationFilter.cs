﻿/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Practices.ServiceLocation;
using Miles.Persistence;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
    /// <summary>
    /// Wraps message in a <see cref="ITransactionContext"/> in which the message is recorded to ensure it is processed only once.
    /// On identifying a message as already processed the message is removed from the queue without doing any work.
    /// </summary>
    /// <remarks>
    /// This assumes a container will have registered itself as an <see cref="IServiceLocator"/> payload to 
    /// retrieve an <see cref="IConsumedRepository"/> instance that will work with the <see cref="ITransactionContext"/>.
    /// </remarks>
    /// <typeparam name="TContext">The type of the consumer.</typeparam>
    /// <seealso cref="global::MassTransit.Pipeline.IFilter{T}" />
    class MessageDeduplicationFilter<TContext> : IFilter<TContext> where TContext : class, ConsumeContext
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("miles-message-deduplication");
        }

        public async Task Send(TContext context, IPipe<TContext> next)
        {
            var container = context.GetPayload<IServiceLocator>();
            var repository = container.GetInstance<IConsumedRepository>();

            var alreadyProcessed = await repository.RecordAsync(context).ConfigureAwait(false);
            if (!alreadyProcessed)
                await next.Send(context).ConfigureAwait(false);
        }
    }
}
