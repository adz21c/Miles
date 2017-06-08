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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    /// <summary>
    /// Base implementation to simulate nested transactions. Inherit and override abstract methods
    /// to perform the transaction commits and rollbacks.
    /// </summary>
    /// <seealso cref="Miles.Persistence.ITransaction" />
    public abstract class TransactionContextBase : ITransactionContext
    {
        private readonly Hook<object, EventArgs> preCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> preRollbackHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postRollbackHook = new Hook<object, EventArgs>();

        /// <summary>
        /// Hook executed prior to commiting the current transaction.
        /// </summary>
        public IHook<object, EventArgs> PreCommit { get { return preCommitHook; } }

        /// <summary>
        /// Hook executed following commiting the current transaction.
        /// </summary>
        public IHook<object, EventArgs> PostCommit { get { return postCommitHook; } }

        /// <summary>
        /// Hook executed prior to a of the current rollback.
        /// </summary>
        public IHook<object, EventArgs> PreRollback { get { return preRollbackHook; } }

        /// <summary>
        /// Hook executed following a of the current rollback.
        /// </summary>
        public IHook<object, EventArgs> PostRollback { get { return postRollbackHook; } }

        private readonly HashSet<TransactionInstance> transactionInstances = new HashSet<TransactionInstance>();
        private bool rollingback = false;

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="hintIsolationLevel">The isolation level hint.</param>
        /// <returns></returns>
        public async Task<ITransaction> BeginAsync(IsolationLevel? hintIsolationLevel = null)
        {
            if (rollingback)
            {
                // If there are no more transaction instances then we've rolled back completely, so time to reset
                if (!transactionInstances.Any())
                    rollingback = false;
                else
                    throw new InvalidOperationException("Cannot create new Transaction instances during Transaction Context rollback");
            }

            // First transaction instance? Then actually create the transaction
            if (!transactionInstances.Any())
                await DoBeginAsync(hintIsolationLevel).ConfigureAwait(false);

            var newTransactionInstance = new TransactionInstance(this);
            transactionInstances.Add(newTransactionInstance);   // register with context

            return newTransactionInstance;
        }

        /// <summary>
        /// Begins the actual transaction.
        /// </summary>
        /// <param name="hintIsolationLevel">The isolation level hint.</param>
        /// <returns></returns>
        protected abstract Task DoBeginAsync(IsolationLevel? hintIsolationLevel = null);

        /// <summary>
        /// Override to perform the actual commit.
        /// </summary>
        /// <returns></returns>
        protected abstract Task DoCommitAsync();

        /// <summary>
        /// Override to perform the actual rollback.
        /// </summary>
        /// <returns></returns>
        protected abstract Task DoRollbackAsync();

        private async Task DoNestedCommitAsync(TransactionInstance instance)
        {
            if (!transactionInstances.Contains(instance))
                throw new InvalidOperationException("Transaction instance is no-longer registered with or does not belong to the Transaction Context");

            if (rollingback)
                throw new InvalidOperationException("Cannot commit while the Transaction Context is rolling back");

            // Only commit when on the outter most transaction
            if (transactionInstances.Count == 1)
            {
                await preCommitHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
                await DoCommitAsync().ConfigureAwait(false);
                await postCommitHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
            }

            instance.Completed = true;
        }

        private async Task DoNestedRollbackAsync(TransactionInstance instance)
        {
            if (!transactionInstances.Contains(instance))
                throw new InvalidOperationException("Transaction instance is no-longer registered with or does not belong to the Transaction Context");

            if (!rollingback)
            {
                rollingback = true;
                await preRollbackHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
                await DoRollbackAsync().ConfigureAwait(false);
                await postRollbackHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
            }

            // we rollback everything and reset so now everything is obsolete
            instance.Completed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (transactionInstances != null && transactionInstances.Any())
            {
                var rollback = DoRollbackAsync();
                if (!rollback.IsCompleted)
                    rollback.RunSynchronously();

                foreach (var instance in transactionInstances)
                    instance.Completed = true;
            }
        }

        class TransactionInstance : ITransaction
        {
            private readonly TransactionContextBase context;

            public TransactionInstance(TransactionContextBase context)
            {
                this.context = context;
            }

            public bool Completed { get; set; } = false;

            /// <summary>
            /// Commits the transaction.
            /// </summary>
            /// <returns></returns>
            public async Task CommitAsync()
            {
                // transaction shouldn't be used anymore
                if (Completed)
                    throw new InvalidOperationException("Transaction instance already completed");

                await context.DoNestedCommitAsync(this).ConfigureAwait(false);
            }

            /// <summary>
            /// Initiates the rollback of the transaction.
            /// </summary>
            /// <returns></returns>
            public async Task RollbackAsync()
            {
                // transaction shouldn't be used anymore
                if (Completed)
                    throw new InvalidOperationException("Transaction instance already completed");

                await context.DoNestedRollbackAsync(this).ConfigureAwait(false);
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (context != null)
                {
                    // Manually rolledback or committed
                    if (!Completed)
                    {
                        // Perform rollback logic synchronously
                        var rollback = context.DoNestedRollbackAsync(this);
                        if (!rollback.IsCompleted)
                            rollback.RunSynchronously();

                        Completed = true;
                    }

                    context.transactionInstances.Remove(this);  // unregister instance
                }
            }
        }
    }
}
