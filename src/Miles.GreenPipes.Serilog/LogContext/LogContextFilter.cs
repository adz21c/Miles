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
using GreenPipes;
using Serilog.Core.Enrichers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SerilogContext = Serilog.Context.LogContext;

namespace Miles.GreenPipes.Serilog.LogContext
{
    class LogContextFilter<TContext> : IFilter<TContext> where TContext : class, PipeContext
    {
        private readonly List<ContextProperty<TContext>> contextProperties;

        public LogContextFilter(List<ContextProperty<TContext>> contextProperties)
        {
            this.contextProperties = contextProperties;
        }

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateFilterScope("miles-serilog-logcontext");
            scope.Add("contextProperties", string.Join(",", contextProperties.Select(x => x.PropertyName)));
        }

        [DebuggerNonUserCode]
        public async Task Send(TContext context, IPipe<TContext> next)
        {
            var pushProperties = contextProperties.Select(x => new PropertyEnricher(x.PropertyName, x.PropertyAccessor(context), x.DestructureObjects)).ToArray();
            using (SerilogContext.PushProperties(pushProperties))
            {
                await next.Send(context).ConfigureAwait(false);
            }
        }
    }
}
