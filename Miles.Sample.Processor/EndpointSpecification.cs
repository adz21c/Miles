using System;
using MassTransit;
using MassTransit.Hosting;
using Microsoft.Practices.Unity;
using System.Linq;
using Miles.MassTransit;
using Miles.Sample.Infrastructure.Unity;
using Miles.MassTransit.Unity;
using Miles.Reflection;
using System.Reflection;
using Miles.Sample.Application;

namespace Miles.Sample.Processor
{
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
            var container = new UnityContainer()
                .ConfigureSample(t => new HierarchicalLifetimeManager())
                .RegisterType<IMessageDispatchProcess, ImmediateMessageDispatchProcess>(new HierarchicalLifetimeManager())
                .RegisterMessageProcessors(() => new HierarchicalLifetimeManager(), AllClasses.FromLoadedAssemblies().GetMessageProcessors());

            configurator.MilesConsumers(container, Assembly.GetAssembly(typeof(FixtureFinishedProcessor)).DefinedTypes.GetProcessedMessageTypes());
        }
    }
}