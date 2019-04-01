using Castle.DynamicProxy;
using GreenPipes;
using System;
using System.Linq;
using System.Reflection;

namespace Miles.GreenPipes
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContextProxyFactory
    {
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();
        
        /// <summary>
        /// Creates a new context proxy class mixing <paramref name="newContextType"/> and <paramref name="existingContextType"/>.
        /// </summary>
        /// <param name="newContextType">The interface representing the new context.</param>
        /// <param name="newContext">Instance that implements <paramref name="newContextType"/> and <see cref="BasePipeContext"/></param>
        /// <param name="existingContextType">The interface representing the current context.</param>
        /// <param name="existingContext">The current context that will be proxied with the <paramref name="newContextType"/></param>
        /// <returns><paramref name="newContextType"/> context mixed with <paramref name="existingContextType"/></returns>
        public static object Create(Type newContextType, PipeContext newContext, Type existingContextType, PipeContext existingContext)
        {
            if (newContextType == null)
                throw new ArgumentNullException(nameof(newContextType));
            if (!newContextType.IsInterface)
                throw new ArgumentOutOfRangeException(nameof(newContextType), "Must be an interface");
            if (!newContextType.GetInterfaces().Contains(typeof(PipeContext)))
                throw new ArgumentOutOfRangeException(nameof(newContextType), $"Must implement {nameof(PipeContext)}");

            if (newContext == null)
                throw new ArgumentNullException(nameof(newContext));
            if (!newContextType.IsInstanceOfType(newContext))
                throw new ArgumentOutOfRangeException(nameof(newContext), $"Must be an instance of {nameof(newContextType)}");

            if (existingContextType == null)
                throw new ArgumentNullException(nameof(existingContextType));
            if (!existingContextType.IsInterface)
                throw new ArgumentOutOfRangeException(nameof(existingContextType), "Must be an interface");
            if (!existingContextType.GetInterfaces().Contains(typeof(PipeContext)))
                throw new ArgumentOutOfRangeException(nameof(existingContextType), $"Must implement {nameof(PipeContext)}");

            if (existingContext == null)
                throw new ArgumentNullException(nameof(existingContext));
            if (!existingContextType.IsInstanceOfType(existingContext))
                throw new ArgumentOutOfRangeException(nameof(existingContext), $"Must be an instance of {nameof(existingContextType)}");
            
            return _proxyGenerator.CreateInterfaceProxyWithTarget(
                newContextType,
                new Type[] { existingContextType },
                newContext,
                new ExistingContextInterceptor(existingContext));
        }

        class ExistingContextInterceptor : IInterceptor
        {
            object _existingContext;

            public ExistingContextInterceptor(object existingContext)
            {
                _existingContext = existingContext;
            }

            public void Intercept(IInvocation invocation)
            {
                // Null invocation target means the target does not implement the interface.
                // In this case we assume the existing context does and any overlap in implementation
                // should be taken care of by the new context
                if (invocation.InvocationTarget != null)
                    invocation.Proceed();
                else
                    invocation.ReturnValue = invocation.Method.Invoke(_existingContext, invocation.Arguments);
            }
        }
    }
}
