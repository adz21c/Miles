﻿/*
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
using Microsoft.Practices.ServiceLocation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.GreenPipes.ContainerScope
{
    /// <summary>
    /// Wraps a pipe context in a container scope/child container
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="GreenPipes.IFilter{TContext}" />
    public class ContainerScopeFilter<TContext> : IFilter<TContext> where TContext : class, PipeContext
    {
        private readonly IScopedServiceLocator _rootServiceLocator;

        public ContainerScopeFilter(IScopedServiceLocator rootServiceLocator)
        {
            _rootServiceLocator = rootServiceLocator;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("miles-container-scope").Add("type", _rootServiceLocator?.ContainerType ?? "Not specified");
        }

        [DebuggerNonUserCode]
        public async Task Send(TContext context, IPipe<TContext> next)
        {
            using (var serviceLocator = GetServiceLocator(context))
            {
                var newContainerScopeContext = ContextProxyFactory.Create(
                    typeof(ContainerScopeContext),
                    new ContainerScopeContextImp(serviceLocator, context),
                    typeof(TContext),
                    context);

                await next.Send((TContext)newContainerScopeContext).ConfigureAwait(false);
            }
        }

        private IScopedServiceLocator GetServiceLocator(TContext context)
        {
            if (context.TryGetPayload(out ContainerScopeContext containerScopeContext))
                return containerScopeContext.ServiceLocator.CreateChildScope();
            else
                return _rootServiceLocator;
        }
    }
}
