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
using GreenPipes;
using Miles.GreenPipes.ContainerScope;
using Miles.Persistence;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.TransactionContext
{
    /// <summary>
    /// Encapsulates a consumer behaviour in a transaction context.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="global::MassTransit.Pipeline.IFilter{TContext}" />
    public class TransactionContextFilter<TContext> : IFilter<TContext> where TContext : class, PipeContext
    {
        private readonly IsolationLevel? hintIsolationLevel;

        public TransactionContextFilter(IsolationLevel? hintIsolationLevel)
        {
            this.hintIsolationLevel = hintIsolationLevel;
        }

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateFilterScope("miles-transaction-context");
            scope.Add("HintIsolationLevel", hintIsolationLevel);
        }

        [DebuggerNonUserCode]
        public async Task Send(TContext context, IPipe<TContext> next)
        {
            // Retrive container controlled instance
            var container = context.GetPayload<ContainerScopeContext>().Container;
            var transactionContext = container.Resolve<ITransactionContext>();

            var transaction = await transactionContext.BeginAsync(hintIsolationLevel).ConfigureAwait(false);
            try
            {
                await next.Send(context).ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
                // Rolling back manually rather than as part of dispose allows us to await
                await transaction.RollbackAsync().ConfigureAwait(false);

                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
