using GreenPipes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.GreenPipes.Junction
{
    /// <summary>
    /// Routes the context through another <see cref="IPipe{TContext}"/> based on matching conditions.
    /// There can be multiple pipes with different conditions (called exits), but the context will be routed to the
    /// first matching pipe.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="GreenPipes.IFilter{TContext}" />
    public class JunctionFilter<TContext> : IFilter<TContext> where TContext : class, PipeContext
    {
        private readonly List<JunctionExit<TContext>> exits;
        private readonly bool continueOnNoMatch;

        public JunctionFilter(List<JunctionExit<TContext>> exits, bool continueOnNoMatch)
        {
            this.exits = exits;
            this.continueOnNoMatch = continueOnNoMatch;
        }

        public void Probe(ProbeContext context)
        {
            var filterContext = context.CreateFilterScope("junction");
            var exitsContext = filterContext.CreateScope("exits");
            exits.ForEach(x => x.Pipe.Probe(exitsContext.CreateScope(x.ConditionDescription)));
        }

        public Task Send(TContext context, IPipe<TContext> next)
        {
            foreach (var exit in exits)
                if (exit.Condition(context))
                    return exit.Pipe.Send(context);

            if (continueOnNoMatch)
                return next.Send(context);

            return Task.CompletedTask;
        }
    }
}
