﻿using GreenPipes;
using MassTransit;
using MassTransit.Configuration;
using MassTransit.ConsumeConfigurators;
using MassTransit.Courier;
using MassTransit.Hosting;
using MassTransit.Saga;
using MassTransit.Saga.SubscriptionConfigurators;
using Miles.MassTransit.Courier;
using System;

namespace Miles.MassTransit.Hosting
{
    public static class ExecuteActivityHostExtensions
    {
        public static void ExecutionActivityHost<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
               where TActivity : class, Activity<TArguments, TLog>, new() // TODO: Overloads
               where TArguments : class
               where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, c => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress))));
        }


        public static void ExecutionActivityHost<TActivity, TArguments>(this IServiceConfigurator configurator, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, new() // TODO: Overloads
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, c => c.ExecuteActivityHost<TActivity, TArguments>())));
        }



        class ReceiveExecutionActivityHostConfigurator<TActivity, TArguments> : IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>
            where TActivity : class, ExecuteActivity<TArguments>, new()  // TODO: Overloads
            where TArguments : class
        {
            private readonly IReceiveEndpointConfigurator receiveEndpointConfigurator;
            private readonly Action<IReceiveEndpointConfigurator> activityHost;

            public ReceiveExecutionActivityHostConfigurator(
                IReceiveEndpointConfigurator receiveEndpointConfigurator,
                Action<IReceiveEndpointConfigurator> activityHost)
            {
                this.receiveEndpointConfigurator = receiveEndpointConfigurator;
                this.activityHost = activityHost;
            }

            public void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            {
                activityHost(receiveEndpointConfigurator);
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
