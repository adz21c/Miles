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
using Miles.DependencyInjection;
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
        private readonly IContainer _rootContainer;

        public ContainerScopeFilter(IContainer rootContainer)
        {
            _rootContainer = rootContainer;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("miles-container-scope").Add("type", _rootContainer?.ContainerType ?? "Not specified");
        }

        [DebuggerNonUserCode]
        public async Task Send(TContext context, IPipe<TContext> next)
        {
            // If a container has been provided, then use that and don't dispose
            if (_rootContainer != null)
            {
                var rootContainerScopeContext = ContextProxyFactory.Create(
                    typeof(ContainerScopeContext),
                    new ContainerScopeContextImp(_rootContainer, context),
                    typeof(TContext),
                    context);
                await next.Send((TContext)rootContainerScopeContext).ConfigureAwait(false);
                return;
            }

            var parentContainer = context.GetPayload<ContainerScopeContext>().Container;
            var containerScopeImp = new ContainerScopeContextImp(context);
            var childContainerScopeContext = ContextProxyFactory.Create(
                typeof(ContainerScopeContext),
                containerScopeImp,
                typeof(TContext),
                context);
            var newContext = (TContext)childContainerScopeContext;

            using (var childContainer = parentContainer.CreateChildScope(c => c.AddSingleton(newContext)))
            {
                containerScopeImp.AssignContainer(childContainer);
                await next.Send(newContext).ConfigureAwait(false);
            }
        }
    }
}
