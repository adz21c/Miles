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
using MassTransit;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.RecordMessageDispatch
{
    /// <summary>
    /// Once a message is dispatched this calls the repository to record the fact.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="global::MassTransit.Pipeline.IFilter{TContext}" />
    class RecordMessageDispatchFilter<TContext> : IFilter<TContext> where TContext : class, SendContext
    {
        private readonly IDispatchRecorder repository;

        public RecordMessageDispatchFilter(IDispatchRecorder repository)
        {
            this.repository = repository;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("miles-record-message-dispatch");
        }

        [DebuggerNonUserCode]
        public async Task Send(TContext context, IPipe<TContext> next)
        {
            await next.Send(context).ConfigureAwait(false);
            await repository.RecordAsync(context).ConfigureAwait(false);
        }
    }
}
