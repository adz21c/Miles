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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.GreenPipes.ServiceScope
{
    /// <summary>
    /// Wraps a pipe context in a container scope/child container
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="GreenPipes.IFilter{TContext}" />
    class ServiceScopeFilter<TContext> : IFilter<TContext> where TContext : class, PipeContext
    {
        public ServiceScopeFilter(IServiceProvider rootServiceProvider)
        {
            RootServiceProvider = rootServiceProvider;
        }

        public IServiceProvider RootServiceProvider { get; }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("service-scope")
                .Add("hasRoot", RootServiceProvider != null);
        }

        [DebuggerNonUserCode]
        public async Task Send(TContext context, IPipe<TContext> next)
        {
            var parentServiceProvider = RootServiceProvider;
            if (parentServiceProvider == null)
            {
                // This will cover off built in MassTransit container support and ServiceScopeContext
                if (!context.TryGetPayload(out parentServiceProvider))
                {
                    // TODO: Exception
                }
            }
            
            var childServiceScopeImp = new ServiceScopeContextImp(context);
            var childServiceScopeContext = ContextProxyFactory.Create(
                typeof(ServiceScopeContext),
                childServiceScopeImp,
                typeof(TContext),
                context);
            var newContext = (TContext)childServiceScopeContext;

            using (var childServiceScope = parentServiceProvider.CreateScope())
            {
                childServiceScopeImp.AssignContainer(childServiceScope.ServiceProvider);
                childServiceScopeImp.GetOrAddPayload(() => childServiceScope.ServiceProvider);

                await next.Send(newContext).ConfigureAwait(false);
            }
        }
    }
}
