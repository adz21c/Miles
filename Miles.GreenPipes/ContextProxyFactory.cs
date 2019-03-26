using Castle.DynamicProxy;
using GreenPipes;
using System;
using System.Linq;

namespace Miles.GreenPipes.UnitTests
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContextProxyFactory
    {
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Creates a context class mixing the new context implementation with the existing context.
        /// </summary>
        /// <param name="newContextType">The interface representing the new context.</param>
        /// <param name="newContextMixin">A class that implements the <paramref name="newContextType"/> and <see cref="BasePipeContext"/> that will be mixed with context</param>
        /// <param name="context">The context that will be proxied</param>
        /// <param name="constructorArgs">Constructor arguments for <paramref name="newContextMixin"/> (must include <paramref name="context"/>)</param>
        /// <returns><paramref name="newContextMixin"/> context mixed with <paramref name="context"/></returns>
        public static object Create(Type newContextType, Type newContextMixin, PipeContext context, params object[] constructorArgs)
        {
            if (newContextType == null)
                throw new ArgumentNullException(nameof(newContextType));

            if (!newContextType.IsInterface)
                throw new ArgumentOutOfRangeException(nameof(newContextType), "Must be an interface");
            if (!newContextType.GetInterfaces().Contains(typeof(PipeContext)))
                throw new ArgumentOutOfRangeException(nameof(newContextType), $"Must implement {nameof(PipeContext)}");

            if (newContextMixin == null)
                throw new ArgumentNullException(nameof(newContextMixin));

            if (!newContextMixin.IsClass)
                throw new ArgumentOutOfRangeException(nameof(newContextMixin), "Must be a class");
            if (newContextMixin.IsGenericTypeDefinition)
                throw new ArgumentOutOfRangeException(nameof(newContextMixin), "Cannot be a generic type definition");
            if (!newContextMixin.GetInterfaces().Contains(newContextType))
                throw new ArgumentOutOfRangeException(nameof(newContextMixin), $"Must implement {nameof(newContextType)} ({newContextType.FullName})");
            if (!newContextMixin.IsSubclassOf(typeof(BasePipeContext)))
                throw new ArgumentOutOfRangeException(nameof(newContextMixin), $"Must implement {nameof(BasePipeContext)}");

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (constructorArgs == null)
                throw new ArgumentNullException(nameof(constructorArgs));

            if (!constructorArgs.Contains(context))
                throw new ArgumentOutOfRangeException(nameof(constructorArgs), $"Must contain {nameof(context)} to be passed to {nameof(BasePipeContext)}");

            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(context);

            return _proxyGenerator.CreateClassProxy(
                newContextMixin,
                new[] { newContextType },
                options,
                constructorArgs);
        }

        /// <summary>
        /// Creates a context class mixing the new context implementation with the existing context.
        /// </summary>
        /// <typeparam name="TNewContext">The interface representing the new context.</typeparam>
        /// <typeparam name="TNewContextMixin">A class that implements the <typeparamref name="newContextType"/> and <see cref="BasePipeContext"/> that will be mixed with context</typeparam>
        /// <param name="context">The context that will be proxied</param>
        /// <param name="constructorArgs">Constructor arguments for <typeparamref name="newContextMixin"/> (must include <paramref name="context"/>)</param>
        /// <returns><typeparamref name="newContextMixin"/> context mixed with <paramref name="context"/></returns>
        public static TNewContextMixin Create<TNewContext, TNewContextMixin>(PipeContext context, params object[] constructorArgs)
            where TNewContextMixin : TNewContext
        {
            return (TNewContextMixin) Create(typeof(TNewContext), typeof(TNewContextMixin), context, constructorArgs);
        }
    }
}
