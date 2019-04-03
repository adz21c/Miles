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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Miles.GreenPipes.ContainerScope;
using System;
using System.Collections.Generic;

namespace Miles.GreenPipes.Unity.ContainerScope
{
    public class UnityScopedServiceLocator : ServiceLocatorImplBase, IScopedServiceLocator
    {
        private IUnityContainer _container;

        public UnityScopedServiceLocator(IUnityContainer container) : base()
        {
            _container = container;
        }

        public string ContainerType => "Unity";
        
        public IScopedServiceLocator CreateChildScope(Type contextType, PipeContext context)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            if (contextType == null)
                throw new ArgumentNullException(nameof(contextType));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (!contextType.IsInstanceOfType(context))
                throw new ArgumentOutOfRangeException(nameof(context), $"Not an instance of {nameof(contextType)}");

            var childContainer = _container.CreateChildContainer();
            try
            {
                childContainer.RegisterInstance(contextType, context, new ExternallyControlledLifetimeManager());

                return new UnityScopedServiceLocator(childContainer);
            }
            catch
            {
                childContainer.Dispose();
                throw;
            }
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            return _container.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            if (_container == null)
                throw new ObjectDisposedException(nameof(_container));

            return _container.ResolveAll(serviceType);
        }

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