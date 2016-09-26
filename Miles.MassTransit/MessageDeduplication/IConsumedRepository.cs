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
using Miles.MassTransit.EnsureMessageDispatch;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
    /// <summary>
    /// Use to record the consumption of messages.
    /// <see cref="RecordMessageDispatchFilter{TContext}"/>.
    /// </summary>
    public interface IConsumedRepository
    {
        /// <summary>
        /// Records a message as consumed.
        /// </summary>
        /// <param name="messageContext">The message context info.</param>
        /// <returns><c>true</c> if the message has already been processed.</returns>
        Task<bool> RecordAsync(MessageContext messageContext);

        /// <summary>
        /// Deletes the old incoming messages.
        /// </summary>
        /// <remarks>
        /// This doesn't need to mean deleting, it could mean archiving. The aim is to keep the data lean for fast processing.
        /// </remarks>
        /// <returns></returns>
        Task DeleteOldRecordsAsync();
    }
}