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
using Miles.MassTransit.TransactionContext;
using System;

namespace MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public static class TransactionContextExtensions
    {
        /// <summary>
        /// Encapsulates the pipe behavior in a <see cref="ITransactionContext" />.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TMessage">The type of the message the consumer will process</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static void UseTransactionContext<TConsumer, TMessage>(this IPipeConfigurator<ConsumerConsumeContext<TConsumer, TMessage>> configurator, Action<ITransactionContextConfigurator> configure = null)
            where TConsumer : class
            where TMessage : class
        {
            var spec = new TransactionContextConfigurator<ConsumerConsumeContext<TConsumer, TMessage>>();
            configure?.Invoke(spec);

            configurator.AddPipeSpecification(spec);
        }

        /// <summary>
        /// Encapsulates the pipe behavior in a <see cref="ITransactionContext" />.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static void UseTransactionContext<TConsumer>(this IPipeConfigurator<ConsumerConsumeContext<TConsumer>> configurator, Action<ITransactionContextConfigurator> configure = null)
            where TConsumer : class
        {
            var spec = new TransactionContextConfigurator<ConsumerConsumeContext<TConsumer>>();
            configure?.Invoke(spec);

            configurator.AddPipeSpecification(spec);
        }
    }
}
