using GreenPipes;
using MassTransit;
using Microsoft.Practices.Unity;
using Miles.MassTransit;
using Miles.MassTransit.EntityFramework.Implementation.RecordMessageDispatch;
using Miles.MassTransit.MessageDispatch;
using Miles.Sample.Application;
using Miles.Sample.Infrastructure.Unity;
using System;
using System.Configuration;
using Topshelf;

namespace Miles.Sample.Processor
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return (int)HostFactory.Run(t =>
            {
                t.Service<ProcessorService>();
                t.RunAsLocalSystem();
                t.SetServiceName("Miles.Sample.Processor");
                t.SetDisplayName("Miles Sample Processor");
            });
        }
    }

    class ProcessorService : ServiceControl
    {
        private IBusControl bus;
        private IUnityContainer container;

        public bool Start(HostControl hostControl)
        {
            container = new UnityContainer()
                .ConfigureSample(t => new HierarchicalLifetimeManager())
                .RegisterType<IActivityContext, ConsumerActivityContext>(new HierarchicalLifetimeManager())
                .RegisterType<IMessageDispatchProcess, MessageDispatchProcess<ConsumeContext>>(new HierarchicalLifetimeManager());

            bus = Bus.Factory.CreateUsingRabbitMq(b =>
            {
                var host = b.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                b.ReceiveEndpoint(host, "Miles.Sample", r =>
                {
                    r.Consumer<FixtureFinishedProcessor>(container, c =>
                    {
                        c.UseContainerScope(container);
                        c.UseTransactionContext();
                        c.UseMessageDeduplication("Miles.Sample");
                    });
                });
            });

            bus.ConnectRecordMessageDispatchObserver(new DispatchRecorder(ConfigurationManager.ConnectionStrings["Miles.Sample"].ConnectionString));

            bus.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            bus.Stop();
            bus = null;

            container.Dispose();
            container = null;

            return true;
        }
    }
}
