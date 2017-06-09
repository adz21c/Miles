using Microsoft.Practices.Unity;
using Miles.MassTransit.EntityFramework;
using Miles.MassTransit.EntityFramework.Implementation.MessageDeduplication;
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;
using Miles.Persistence;
using Miles.Sample.Persistence.EF;
using System;
using System.Data.Entity;
using System.Linq;

namespace Miles.Sample.Infrastructure.Unity
{
    public static class DIConfig
    {
        public static IUnityContainer ConfigureSample(this IUnityContainer container, Func<Type, LifetimeManager> lifetimeManager)
        {
            container.RegisterTypes(
                RegistrationByConvention.FromAssembliesInSearchPath().Where(x => x.Namespace.StartsWith("Miles.Sample")),
                t => WithMappings.FromMatchingInterface(t),
                WithName.Default,
                lifetimeManager);

            // Miles.MassTransit EF repositories
            container.RegisterTypes(
                RegistrationByConvention.FromAssembliesInSearchPath().Where(x => x.Namespace.StartsWith("Miles.MassTransit.EntityFramework.Repositories")),
                t => WithMappings.FromMatchingInterface(t),
                WithName.Default,
                lifetimeManager);

            container.RegisterType<DbContext, SampleDbContext>(lifetimeManager(typeof(SampleDbContext)));

            container
                // Miles
                .RegisterInstance<ITime>(new Time())
                // Miles.MassTransit
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>(lifetimeManager(typeof(TransactionalMessagePublisher)))
                .RegisterType<ICommandPublisher, TransactionalMessagePublisher>(lifetimeManager(typeof(TransactionalMessagePublisher)))
                .RegisterType<TransactionalMessagePublisher>(lifetimeManager(typeof(TransactionalMessagePublisher)))
                // Miles.MassTransit.EntityFramework
                .RegisterType<IOutgoingMessageRepository, OutgoingMessageRepository>(lifetimeManager(typeof(OutgoingMessageRepository)))
                .RegisterType<IConsumptionRecorder, ConsumptionRecorder>(lifetimeManager(typeof(ConsumptionRecorder)))
                .RegisterType<ITransactionContext, EFTransactionContext>(lifetimeManager(typeof(EFTransactionContext)));

            return container;
        }
    }
}
