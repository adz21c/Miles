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
using MassTransit.ConsumeConfigurators;
using Miles.MassTransit.Consumers;
using Miles.MassTransit.ContainerScope;
using System;

namespace MassTransit
{
    public static class MilesConsumerExtensions
    {
        /// <summary>
        /// Connect a consumer to the receiving endpoint constructing the consumer using <see cref="ContainerConsumerFactory{TConsumer}"/>.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator"></param>
        /// <param name="containerStackFactory">The container stack factory.</param>
        /// <param name="configure">Optional, configure the consumer.</param>
        public static void Consumer<TConsumer>(this IReceiveEndpointConfigurator configurator, IContainerStackFactory containerStackFactory, Action<IConsumerConfigurator<TConsumer>> configure = null)
            where TConsumer : class, IConsumer
        {
            var consumerFactory = new ContainerConsumerFactory<TConsumer>(containerStackFactory);

            configurator.Consumer(consumerFactory, configure);
        }
    }
}
