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
using MassTransit.ConsumeConfigurators;
using Miles.MassTransit.TransactionContext;
using Miles.Persistence;
using System;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class TransactionContextExtensions
    {
        /// <summary>
        /// Encapsulates the pipe behavior in a <see cref="ITransactionContext"/>.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IConsumerConfigurator<TConsumer> UseTransactionContext<TConsumer>(this IConsumerConfigurator<TConsumer> configurator, Action<ITransactionContextConfigurator> configure = null)
            where TConsumer : class, IConsumer
        {
            var spec = new TransactionContextSpecification<TConsumer>();
            configure?.Invoke(spec);

            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
