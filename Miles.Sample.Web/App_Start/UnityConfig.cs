using MassTransit;
using Microsoft.Practices.Unity;
using Miles.MassTransit;
using Miles.MassTransit.MessageDispatch;
using Miles.MassTransit.Unity;
using Miles.Persistence;
using Miles.Sample.Infrastructure.Unity;
using Miles.Sample.Persistence.EF;
using System;
using System.Linq;

namespace Miles.Sample.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.ConfigureSample(t => new PerRequestLifetimeManager());

            container.RegisterType<IMessageDispatchProcess, MessageDispatchProcess>(new PerRequestLifetimeManager());
            container.RegisterInstance<IBus>(MassTransitBusConfig.GetBus());
            container.RegisterInstance<IPublishEndpoint>(MassTransitBusConfig.GetBus());
        }
    }
}
