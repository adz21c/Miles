using System;
using Microsoft.Practices.Unity;
using Miles.DependencyInjection;

namespace Miles.Unity.DependencyInjection
{
    public class UnityContainerBuilder : IContainerBuilder
    {
        private readonly IUnityContainer _container;

        public UnityContainerBuilder(IUnityContainer container)
        {
            _container = container;
        }

        public IContainerBuilder AddScoped(Type service)
        {
            _container.RegisterType(service, new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped<TService>()
        {
            _container.RegisterType<TService>(new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped(Type service, Type implementation)
        {
            _container.RegisterType(service, implementation, new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped<TService, TImplementation>() where TImplementation : TService
        {
            _container.RegisterType<TService, TImplementation>(new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped(Type service, Func<object> implementationFactory)
        {
            _container.RegisterType(service, new HierarchicalLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddScoped<TService>(Func<TService> implementationFactory)
        {
            _container.RegisterType<TService>(new HierarchicalLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddSingleton(Type service)
        {
            _container.RegisterType(service, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton<TService>()
        {
            _container.RegisterType<TService>(new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton(Type service, Type implementation)
        {
            _container.RegisterType(service, implementation, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _container.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton(Type service, Func<object> implementationFactory)
        {
            _container.RegisterType(service, new ContainerControlledLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddSingleton<TService>(Func<TService> implementationFactory)
        {
            _container.RegisterType<TService>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddSingleton(Type service, object instance)
        {
            _container.RegisterInstance(service, instance, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton<TService>(TService instance)
        {
            _container.RegisterInstance(instance, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient(Type service)
        {
            _container.RegisterType(service, new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient<TService>()
        {
            _container.RegisterType<TService>(new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient(Type service, Type implementation)
        {
            _container.RegisterType(service, implementation, new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            _container.RegisterType<TService, TImplementation>(new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient(Type service, Func<object> implementationFactory)
        {
            _container.RegisterType(service, new TransientLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddTransient<TService>(Func<TService> implementationFactory)
        {
            _container.RegisterType<TService>(new TransientLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }
    }
}
