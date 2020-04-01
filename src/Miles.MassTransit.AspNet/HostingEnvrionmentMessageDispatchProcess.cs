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
using MassTransit;
using Miles.MassTransit.MessageDispatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Miles.MassTransit.AspNet
{
    public class HostingEnvrionmentMessageDispatchProcess<TBus> : IMessageDispatchProcess where TBus : ISendEndpointProvider, IPublishEndpoint
    {
        private readonly MessageDispatchProcess<TBus> process;

        public HostingEnvrionmentMessageDispatchProcess(TBus bus)
        {
            this.process = new MessageDispatchProcess<TBus>(bus);
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
                await process.ExecuteAsync(messagesForDispatch).ConfigureAwait(false);
            });

            return Task.CompletedTask;
        }
    }
}
