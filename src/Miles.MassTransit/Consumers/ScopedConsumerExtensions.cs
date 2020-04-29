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
using MassTransit.ConsumeConfigurators;
using Miles.MassTransit.Consumers;
using System;

namespace MassTransit
{
    public static class ScopedConsumerExtensions
    {
        /// <summary>
        /// Connect a consumer to the receiving endpoint constructing the consumer using <see cref="ContainerConsumerFactory{TConsumer}"/>.
        /// Expected there will be a <see cref="Miles.GreenPipes.ContainerScope.ServiceScopeContext"/> before this.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator"></param>
        /// <param name="configure">Optional, configure the consumer.</param>
        public static void ScopedConsumer<TConsumer>(this IReceiveEndpointConfigurator configurator, Action<IConsumerConfigurator<TConsumer>> configure = null)
            where TConsumer : class, IConsumer
        {
            var consumerFactory = new ContainerConsumerFactory<TConsumer>();

            configurator.Consumer(consumerFactory, configure);
        }
    }
}
