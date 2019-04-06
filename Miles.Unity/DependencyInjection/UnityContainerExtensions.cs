using Microsoft.Practices.Unity;
using Miles.DependencyInjection;

namespace Miles.Unity.DependencyInjection
{
    public static class UnityContainerExtensions
    {
        public static IContainerBuilder ToContainerBuilder(this IUnityContainer container)
        {
            return new UnityContainerWrapper(container);
        }

        public static IContainer ToContainer(this IUnityContainer container)
        {
            return new UnityContainerWrapper(container);
        }
    }
}
