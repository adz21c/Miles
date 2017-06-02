/*
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDispatch
{
    /// <summary>
    /// Default implementation of <see cref="IMessageDispatchProcess"/> that immediately dispatches the messages.
    /// </summary>
    /// <seealso cref="IMessageDispatchProcess" />
    public class MessageDispatchProcess : IMessageDispatchProcess
    {
        private readonly IMessageDispatcher commandDispatcher;
        private readonly ConventionBasedMessageDispatcher eventDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatchProcess"/> class.
        /// </summary>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public MessageDispatchProcess(
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
        public async Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            foreach (var message in messages)
            {
                var dispatcher = message.ConceptType == OutgoingMessageConceptType.Command ? commandDispatcher : eventDispatcher;
                await dispatcher.DispatchAsync(message).ConfigureAwait(false);
            }
        }
    }
}
