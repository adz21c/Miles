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
using Miles.GreenPipes.ContainerScope;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.Consumers
{
    /// <summary>
    /// Creates consumers using the <see cref="IContainer"/> registered before it via a <see cref="ContainerScopeContext"/>.
    /// </summary>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    /// <seealso cref="MassTransit.IConsumerFactory{TConsumer}" />
    class ContainerConsumerFactory<TConsumer> : IConsumerFactory<TConsumer> where TConsumer : class
    {
        public void Probe(ProbeContext context)
        {
            context.CreateConsumerFactoryScope<TConsumer>("miles-container");
        }

        [DebuggerNonUserCode]
        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next) where T : class
        {
            var container = context.GetPayload<ContainerScopeContext>().Container;
            var consumer = container.Resolve<TConsumer>();
            if (consumer == null)
                throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");

            var consumerConsumeContext = context.PushConsumerScope(consumer, container);

            await next.Send(consumerConsumeContext).ConfigureAwait(false);
        }
    }
}
