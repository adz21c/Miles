﻿/*
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
using MassTransit;
using Microsoft.Practices.ServiceLocation;
using Miles.MassTransit.MessageDeduplication;
using Miles.Persistence;
using System;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageDeduplicationExtensions
    {
        /// <summary>
        /// The message is recorded to ensure it is processed only once.
        /// On identifying a message as already processed the message is removed from the queue without doing any work.
        /// This should be wrapped in an <see cref="ITransactionContext"/> to ensure the processing and recording
        /// of the message are a single unit of work.
        /// </summary>
        /// <remarks>
        /// This assumes a container will have registered itself as an <see cref="IServiceLocator"/> payload to 
        /// retrieve an <see cref="IConsumedRepository"/> instance that will work with the <see cref="ITransactionContext"/>.
        /// </remarks>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IPipeConfigurator<ConsumerConsumeContext<TConsumer>> UseMessageDeduplication<TConsumer>(this IPipeConfigurator<ConsumerConsumeContext<TConsumer>> configurator, Action<IMessageDeduplicationConfigurator> configure = null)
            where TConsumer : class
        {
            var config = new MessageDeduplicationConfigurator();
            configure?.Invoke(config);
            var spec = new MessageDeduplicationSpecification<ConsumerConsumeContext<TConsumer>>(config);
            configurator.AddPipeSpecification(spec);
            return configurator;
        }

        public static IPipeConfigurator<ConsumeContext<TMessage>> UseMessageDeduplication<TMessage>(this IPipeConfigurator<ConsumeContext<TMessage>> configurator, Action<IMessageDeduplicationConfigurator> configure = null)
            where TMessage : class
        {
            var config = new MessageDeduplicationConfigurator();
            configure?.Invoke(config);
            var spec = new MessageDeduplicationSpecification<ConsumeContext<TMessage>>(config);
            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
