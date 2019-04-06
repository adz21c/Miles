using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Miles.DependencyInjection;

namespace Miles.Unity.DependencyInjection
{
    public class UnityContainerWrapper : IContainerBuilder, IContainer
    {
        private IUnityContainer _container;

        public UnityContainerWrapper(IUnityContainer container)
        {
            _container = container;
        }

        public string ContainerType => "Unity";

        #region IContainerBuilder

        public IContainerBuilder AddScoped(Type service)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped<TService>()
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService>(new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped(Type service, Type implementation)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, implementation, new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped<TService, TImplementation>() where TImplementation : TService
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService, TImplementation>(new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerBuilder AddScoped(Type service, Func<object> implementationFactory)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, new HierarchicalLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddScoped<TService>(Func<TService> implementationFactory)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService>(new HierarchicalLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddSingleton(Type service)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton<TService>()
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService>(new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton(Type service, Type implementation)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, implementation, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton(Type service, Func<object> implementationFactory)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, new ContainerControlledLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddSingleton<TService>(Func<TService> implementationFactory)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddSingleton(Type service, object instance)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterInstance(service, instance, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddSingleton<TService>(TService instance)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterInstance(instance, new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient(Type service)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient<TService>()
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService>(new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient(Type service, Type implementation)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, implementation, new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService, TImplementation>(new TransientLifetimeManager());
            return this;
        }

        public IContainerBuilder AddTransient(Type service, Func<object> implementationFactory)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType(service, new TransientLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainerBuilder AddTransient<TService>(Func<TService> implementationFactory)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            _container.RegisterType<TService>(new TransientLifetimeManager(), new InjectionFactory(c => implementationFactory()));
            return this;
        }

        public IContainer CreateContainer() => this;

        #endregion

        #region IContainer

        public object Resolve(Type serviceType, string name = null)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            if (name == null)
                return _container.Resolve(serviceType);
            else
                return _container.Resolve(serviceType, name);
        }

        public TService Resolve<TService>(string name = null)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            if (name == null)
                return _container.Resolve<TService>();
            else
                return _container.Resolve<TService>(name);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            return _container.ResolveAll(serviceType);
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            return _container.ResolveAll<TService>();
        }

        public IContainerBuilder CreateChildScopeBuilder()
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            return new UnityContainerWrapper(_container.CreateChildContainer());
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _container.Dispose();
                    _container = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
