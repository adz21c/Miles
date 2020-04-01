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
    public static class ReceiveExecuteActivityHostExtensions
    {
        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateHostAddress">The compensate host address.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IBusFactoryConfigurator configurator, Uri compensateHostAddress, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>, new()
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateHostAddress">The compensate host address.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IBusFactoryConfigurator configurator, Uri compensateHostAddress, Func<TActivity> controllerFactory, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, controllerFactory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateHostAddress">The compensate host address.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IBusFactoryConfigurator configurator, Uri compensateHostAddress, Func<TArguments, TActivity> controllerFactory, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, controllerFactory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <typeparam name="TLog">The compensation log type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateHostAddress">The compensate host address.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IBusFactoryConfigurator configurator, Uri compensateHostAddress, ExecuteActivityFactory<TActivity, TArguments> factory, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, factory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments>(this IBusFactoryConfigurator configurator, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, new()
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateAddress">The compensate address.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments>(this IBusFactoryConfigurator configurator, Uri compensateAddress, Func<TActivity> controllerFactory, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(compensateAddress, controllerFactory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateAddress">The compensate address.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments>(this IBusFactoryConfigurator configurator, Uri compensateAddress, Func<TArguments, TActivity> controllerFactory, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(compensateAddress, controllerFactory, ac))));
        }

        /// <summary>
        /// Creates a receive endpoint and configures an activity specifying queues by convention.
        /// </summary>
        /// <typeparam name="TActivity">The activity processor.</typeparam>
        /// <typeparam name="TArguments">The arguments DTO type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compensateAddress">The compensate address.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="configure">The configure.</param>
        public static void ExecuteActivityHost<TActivity, TArguments>(this IBusFactoryConfigurator configurator, Uri compensateAddress, ExecuteActivityFactory<TActivity, TArguments> factory, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecuteActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(compensateAddress, factory, ac))));
        }

        class ReceiveExecuteActivityHostConfigurator<TActivity, TArguments> : IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            private readonly IReceiveEndpointConfigurator receiveEndpointConfigurator;
            private readonly Action<IReceiveEndpointConfigurator, Action<IExecuteActivityConfigurator<TActivity, TArguments>>> activityHost;

            public ReceiveExecuteActivityHostConfigurator(
                IReceiveEndpointConfigurator receiveEndpointConfigurator,
                Action<IReceiveEndpointConfigurator, Action<IExecuteActivityConfigurator<TActivity, TArguments>>> activityHost)
            {
                this.receiveEndpointConfigurator = receiveEndpointConfigurator;
                this.activityHost = activityHost;
            }

            public void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
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
