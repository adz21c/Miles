using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.DependencyInjection
{
    public interface IContainer : IDisposable
    {
        string ContainerType { get; }

        IContainerBuilder CreateChildScopeBuilder();

        object Resolve(Type serviceType, string name = null);

        TService Resolve<TService>(string name = null);

        IEnumerable<object> ResolveAll(Type serviceType);

        IEnumerable<TService> ResolveAll<TService>();
    }

    public static class ScopedServiceLocatorExtensions
    {
        public static IContainer CreateChildScope(this IContainer container, Action<IContainerBuilder> configurator = null)
        {
            var builder = container.CreateChildScopeBuilder();
            configurator?.Invoke(builder);
            return builder.CreateContainer();
        }
    }
}
