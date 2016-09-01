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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Interface to the storage mechanism of <see cref="OutgoingMessage" />.
    /// </summary>
    /// <remarks>
    /// The idea is this will sit within the transaction of a handler unless otherwise specified.
    /// </remarks>
    public interface IOutgoingMessageRepository
    {
        /// <summary>
        /// Saves the specified events to the storage mechanism.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        Task SaveAsync(IEnumerable<OutgoingMessage> messages);

        /// <summary>
        /// Records a message as dispatched at the supplied time.
        /// </summary>
        /// <param name="when">When the message was dispatched.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// This should not sit within the current transaction context.
        /// The message is gone, we don't want this rolled back on failure.
        /// </remarks>
        Task RecordMessageDispatchAsync(DateTime when, Guid messageId);

        /// <summary>
        /// Records messages as dispatched at the supplied time.
        /// </summary>
        /// <param name="when">When the message was dispatched.</param>
        /// <param name="messageIds">The message identifiers.</param>
        /// <returns></returns>
        /// <remarks>
        /// This should not sit within the current transaction context.
        /// The messages are gone, we don't want this rolled back on failure.
        /// </remarks>
        Task RecordMessageDispatchAsync(DateTime when, IEnumerable<Guid> messageIds);

        /// <summary>
        /// Deletes the old dispatched outgoing messages.
        /// </summary>
        /// <returns></returns>
        Task DeleteOldDispatchedAsync();
    }
}
