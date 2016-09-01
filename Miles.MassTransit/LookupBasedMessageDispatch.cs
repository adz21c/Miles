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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches messages to a specific endpoint by looking up the endpoint uri
    /// based on the message type.
    /// </summary>
    /// <remarks>Uses MassTransit's Send method.</remarks>
    /// <seealso cref="Miles.MassTransit.IMessageDispatcher" />
    public class LookupBasedMessageDispatch : IMessageDispatcher
    {
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly ILookupEndpointUri endpointUriLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupBasedMessageDispatch" /> class.
        /// </summary>
        /// <param name="endpointUriLookup">The endpoint URI lookup.</param>
        /// <param name="sendEndpointProvider">The send endpoint provider.</param>
        public LookupBasedMessageDispatch(ILookupEndpointUri endpointUriLookup, ISendEndpointProvider sendEndpointProvider)
        {
            this.endpointUriLookup = endpointUriLookup;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageDetails">The message details.</param>
        /// <returns></returns>
        public async Task DispatchAsync(object message, OutgoingMessage messageDetails)
        {
            var endpointUri = await endpointUriLookup.LookupAsync(message.GetType()).ConfigureAwait(false);
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(endpointUri).ConfigureAwait(false);
            await sendEndpoint.Send(message, c => { c.MessageId = messageDetails.MessageId; c.CorrelationId = messageDetails.CorrelationId; }).ConfigureAwait(false);
        }

        public Task DispatchAsync(IEnumerable<OutgoingMessageForDispatch> ignored)
        {
            throw new NotImplementedException();
        }
    }
}
