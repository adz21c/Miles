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
using MassTransit;
using MassTransit.Util;
using Microsoft.Practices.ServiceLocation;
using Miles.GreenPipes.ContainerScope;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.Consumers
{
    /// <summary>
    /// Creates consumers using the <see cref="IServiceLocator"/> attached to the context. If one does not exist
    /// then create the <see cref="IContainerStack"/> as per ContainerScope. If creating a <see cref="IContainerStack"/>
    /// then create an initial child scope (as per ContainerScope), but if not then don't create a child scope and
    /// defer to the developers use of ContainerScope to create appropriate child scopes.
    /// </summary>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    /// <seealso cref="MassTransit.IConsumerFactory{TConsumer}" />
    public class ContainerConsumerFactory<TConsumer> : IConsumerFactory<TConsumer> where TConsumer : class
    {
        private readonly IContainerStackFactory containerStackFactory;

        public ContainerConsumerFactory(IContainerStackFactory containerStackFactory)
        {
            this.containerStackFactory = containerStackFactory;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateConsumerFactoryScope<TConsumer>("miles-container")
                .Add("container-type", containerStackFactory.ContainerType);
        }

        [DebuggerNonUserCode]
        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next) where T : class
        {
            var createdStack = false;
            var containerStack = context.GetOrAddPayload(() =>
            {
                if (containerStackFactory == null)
                    throw new InvalidOperationException("No container stack factory. Make sure the first ContainerScope encountered has a factory to setup the initial container.");

                createdStack = true;
                var stack = containerStackFactory.Create(context);
                context.GetOrAddPayload<IServiceLocator>(() => stack);
                return stack;
            });
            var container = containerStack as IServiceLocator;

            // If the stack was just created then we need to play ContainerScope and create a 
            // child scope for the consumer to live in, else we assume the developer has manually
            // created the container scope as per their requirements.
            if (createdStack)
            {
                containerStack.PushScope(context);
                try
                {
                    await ResolveAndExecuteConsumer(context, next, container).ConfigureAwait(false);
                }
                finally
                {
                    containerStack.PopScope();
                }
            }
            else
                await ResolveAndExecuteConsumer(context, next, container).ConfigureAwait(false);
        }

        private static async Task ResolveAndExecuteConsumer<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next, IServiceLocator container) where T : class
        {
            var consumer = container.GetInstance<TConsumer>();
            if (consumer == null)
                throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");

            await next.Send(context.PushConsumer(consumer)).ConfigureAwait(false);
        }
    }
}
