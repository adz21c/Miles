/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
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
using Miles.MassTransit.MessageDispatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Miles.MassTransit.AspNet
{
    public class HostingEnvrionmentMessageDispatchProcess : IMessageDispatchProcess
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IEventDispatcher eventDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingEnvrionmentMessageDispatchProcess"/> class.
        /// </summary>
        /// <remarks>
        /// This, and its dependencies, should be created as a process singleton.
        /// The expectation is this will live on outside of a message request.
        /// </remarks>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public HostingEnvrionmentMessageDispatchProcess(
            ICommandDispatcher commandDispatcher,
            IEventDispatcher eventDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
            this.eventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// Initiates the dispatch of messages to the message queue.
        /// </summary>
        /// <remarks>
        /// Assume control of the messages are being handed over to this process.
        /// </remarks>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            var messagesForDispatch = messages.ToList();
            HostingEnvironment.QueueBackgroundWorkItem(async cancellationToken =>
            {
                foreach (var message in messagesForDispatch)
                {
                    if (message.ConceptType == OutgoingMessageConceptType.Command)
                        await commandDispatcher.DispatchAsync(message).ConfigureAwait(false);
                    else
                        await eventDispatcher.DispatchAsync(message).ConfigureAwait(false);
                }
            });

            return Task.CompletedTask;
        }
    }
}
