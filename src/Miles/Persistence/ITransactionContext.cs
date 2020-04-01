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
using System;
using System.Data;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    /// <summary>
    /// Represents a transaction throughout a request. Simulates nesting of transactions
    /// such that <see cref="Miles.Messaging.IMessageProcessor{TMessage}"/> can be nested 
    /// and transaction behaviour is not confusing.
    /// 
    /// Ultimately this has events that the <see cref="Miles.Messaging.IEventPublisher"/> and
    /// <see cref="Miles.Messaging.ICommandPublisher"/> monitor to release events to message
    /// queues or whatever mechanism exists.
    /// </summary>
    public interface ITransactionContext : IDisposable
    {
        /// <summary>
        /// Creates a new transaction within the context.
        /// </summary>
        /// <param name="hintIsolationLevel">The isolation level hint.</param>
        /// <returns></returns>
        Task<ITransaction> BeginAsync(IsolationLevel? hintIsolationLevel = null);

        /// <summary>
        /// Hook executed prior to commiting the current transaction.
        /// </summary>
        IHook<object, EventArgs> PreCommit { get; }

        /// <summary>
        /// Hook executed following commiting the current transaction.
        /// </summary>
        IHook<object, EventArgs> PostCommit { get; }

        /// <summary>
        /// Hook executed prior to a of the current rollback.
        /// </summary>
        IHook<object, EventArgs> PreRollback { get; }

        /// <summary>
        /// Hook executed following a of the current rollback.
        /// </summary>
        IHook<object, EventArgs> PostRollback { get; }
    }
}
