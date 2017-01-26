﻿using GreenPipes;
using MassTransit;
using MassTransit.Configuration;
using MassTransit.ConsumeConfigurators;
using MassTransit.Courier;
using MassTransit.Saga;
using MassTransit.Saga.SubscriptionConfigurators;
using Miles.MassTransit.Courier;
using System;

namespace Miles.MassTransit.Courier
{
    public static class ReceiveCompensateActivityHostExtensions
    {
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>, new()
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(ac))));
        }

        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, Func<TActivity> controllerFactory, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(controllerFactory, ac))));
        }

        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, Func<TLog, TActivity> controllerFactory, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveCompensateActivityHostConfigurator<TActivity, TLog>(r, (c, ac) => c.CompensateActivityHost<TActivity, TLog>(controllerFactory, ac))));
        }

        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, CompensateActivityFactory<TActivity, TLog> factory, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TLog).GenerateExecutionQueueName(),
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