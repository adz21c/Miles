using Miles.GreenPipes.Junction;
using System;

namespace GreenPipes
{
    public static class JunctionExtensions
    {
        /// <summary>
        /// Routes the context through another <see cref="IPipe{TContext}"/> based on matching conditions.
        /// There can be multiple pipes with different conditions (called exits), but the context will be routed to the
        /// first matching pipe.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">Configures the junction.</param>
        public static void UseJunction<TContext>(this IPipeConfigurator<TContext> configurator, Action<IJunctionConfigurator<TContext>> configure) where TContext : class, PipeContext
        {
            var spec = new JunctionSpecification<TContext>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
        }
    }
}
