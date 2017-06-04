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
using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDispatch
{
    /// <summary>
    /// Dispatches messages based on the contract type to the message queue.
    /// </summary>
    /// <remarks>Uses MassTransit's Publish method.</remarks>
    /// <seealso cref="ICommandDispatcher" />
    public class PublishMessageDispatcher : ICommandDispatcher, IEventDispatcher
    {
        private readonly IPublishEndpoint publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionBasedMessageDispatcher" /> class.
        /// </summary>
        /// <param name="publishEndpoint">The publish endpoint.</param>
        public PublishMessageDispatcher(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Task DispatchAsync(OutgoingMessageForDispatch message)
        {
            return publishEndpoint.Publish(message.MessageObject, c => { c.MessageId = message.MessageId; c.CorrelationId = message.CorrelationId; });
        }
    }
}
