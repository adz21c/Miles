namespace Miles.Sample.Processor
{
    using Application;
    using global::GreenPipes;
    using global::MassTransit;
    using global::MassTransit.Hosting;
    using Infrastructure.Unity;
    using MassTransit.EntityFramework.Implementation.RecordMessageDispatch;
    using Microsoft.Practices.Unity;
    using Miles.MassTransit;
    using Miles.MassTransit.MessageDispatch;
    using System;
    using System.Configuration;

    /// <summary>
    /// Configures an endpoint for the assembly
    /// </summary>
    public class EndpointSpecification : IEndpointSpecification
    {
        /// <summary>
        /// The default queue name for the endpoint, which can be overridden in the .config 
        /// file for the assembly
        /// </summary>
        public string QueueName
        {
            get { return "Miles.Sample"; }
        }

        /// <summary>
        /// The default concurrent consumer limit for the endpoint, which can be overridden in the .config 
        /// file for the assembly
        /// </summary>
        public int ConsumerLimit
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        /// Configures the endpoint, with consumers, handlers, sagas, etc.
        /// </summary>
        public void Configure(IReceiveEndpointConfigurator configurator)
        {
            // message consumers, middleware, etc. are configured here
            var container = new UnityContainer()
                .ConfigureSample(t => new HierarchicalLifetimeManager())
                // Miles.MassTransit
                .RegisterType<IActivityContext, ConsumerActivityContext>(new HierarchicalLifetimeManager())
                .RegisterType<IEventDispatcher, PublishMessageDispatcher>(new HierarchicalLifetimeManager())
                .RegisterType<ICommandDispatcher, PublishMessageDispatcher>(new HierarchicalLifetimeManager())
                .RegisterType<IMessageDispatchProcess, MessageDispatchProcess>(new HierarchicalLifetimeManager())
                ;

            configurator.UseRecordMessageDispatch(new DispatchRecorder(ConfigurationManager.ConnectionStrings["Miles.Sample"].ConnectionString));

            configurator.Consumer<FixtureFinishedProcessor>(container, c =>
            {
                c.UseContainerScope(container);
                c.UseTransactionContext();
                c.UseMessageDeduplication("Miles.Sample");
            });
        }
    }
}
