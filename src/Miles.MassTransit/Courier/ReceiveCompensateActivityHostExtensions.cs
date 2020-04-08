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
using MassTransit.Configuration;
using MassTransit.ConsumeConfigurators;
using MassTransit.Courier;
using MassTransit.Saga;
using MassTransit.Saga.SubscriptionConfigurators;
using Miles.MassTransit.Courier;
using System;

namespace MassTransit
{
    public static class ReceiveCompensateActivityHostExtensions
    {
        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator"></param>
        /// <param name="configure"></param>
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>, new()
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateCompensationQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="configure">The configure.</param>
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, Func<TActivity> controllerFactory, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateCompensationQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(controllerFactory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="configure">The configure.</param>
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, Func<TLog, TActivity> controllerFactory, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateCompensationQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(controllerFactory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="configure">The configure.</param>
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, CompensateActivityFactory<TActivity, TLog> factory, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateCompensationQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(factory, ac))));
        }

        class ReceiveCompensateActivityHostConfigurator<TActivity, TLog> : IReceiveCompensateActivityHostConfigurator<TActivity, TLog>
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            private readonly IReceiveEndpointConfigurator receiveEndpointConfigurator;
            private readonly Action<IReceiveEndpointConfigurator, Action<ICompensateActivityConfigurator<TActivity, TLog>>> activityHost;

            public ReceiveCompensateActivityHostConfigurator(
                IReceiveEndpointConfigurator receiveEndpointConfigurator,
                Action<IReceiveEndpointConfigurator, Action<ICompensateActivityConfigurator<TActivity, TLog>>> activityHost)
            {
                this.receiveEndpointConfigurator = receiveEndpointConfigurator;
                this.activityHost = activityHost;
            }

            public void Activity(Action<ICompensateActivityConfigurator<TActivity, TLog>> configure = null)
            {
                activityHost(receiveEndpointConfigurator, configure);
            }

            #region Adapter

            public Uri InputAddress => receiveEndpointConfigurator.InputAddress;

            public void AddEndpointSpecification(IReceiveEndpointSpecification configurator)
            {
                receiveEndpointConfigurator.AddEndpointSpecification(configurator);
            }

            public void AddPipeSpecification(IPipeSpecification<ConsumeContext> specification)
            {
                receiveEndpointConfigurator.AddPipeSpecification(specification);
            }

            public void AddPipeSpecification<T>(IPipeSpecification<ConsumeContext<T>> specification) where T : class
            {
                receiveEndpointConfigurator.AddPipeSpecification(specification);
            }

            public void ConfigurePublish(Action<IPublishPipeConfigurator> callback)
            {
                receiveEndpointConfigurator.ConfigurePublish(callback);
            }

            public void ConfigureSend(Action<ISendPipeConfigurator> callback)
            {
                receiveEndpointConfigurator.ConfigureSend(callback);
            }

            public ConnectHandle ConnectConsumerConfigurationObserver(IConsumerConfigurationObserver observer)
            {
                return receiveEndpointConfigurator.ConnectConsumerConfigurationObserver(observer);
            }

            public ConnectHandle ConnectSagaConfigurationObserver(ISagaConfigurationObserver observer)
            {
                return receiveEndpointConfigurator.ConnectSagaConfigurationObserver(observer);
            }

            public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator) where TConsumer : class
            {
                receiveEndpointConfigurator.ConsumerConfigured(configurator);
            }

            public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
                where TConsumer : class
                where TMessage : class
            {
                receiveEndpointConfigurator.ConsumerMessageConfigured(configurator);
            }

            void ISagaConfigurationObserver.SagaConfigured<TSaga>(ISagaConfigurator<TSaga> configurator)
            {
                receiveEndpointConfigurator.SagaConfigured(configurator);
            }

            void ISagaConfigurationObserver.SagaMessageConfigured<TSaga, TMessage>(ISagaMessageConfigurator<TSaga, TMessage> configurator)
            {
                receiveEndpointConfigurator.SagaMessageConfigured(configurator);
            }

            #endregion
        }
    }
}
