using GreenPipes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Miles.GreenPipes.Junction
{
    public class JunctionSpecification<TContext> : IJunctionConfigurator<TContext>, IPipeSpecification<TContext> where TContext : class, PipeContext
    {
        private readonly List<JunctionExit<TContext>> exits = new List<JunctionExit<TContext>>();
        private bool continueOnNoMatch = false;

        void IJunctionConfigurator<TContext>.Exit(IPipe<TContext> pipe, Expression<Func<TContext, bool>> condition)
        {
            exits.Add(new JunctionExit<TContext>(condition.ToString(), condition.Compile(), pipe));
        }

        void IJunctionConfigurator<TContext>.Exit(string probeContextName, IPipe<TContext> pipe, Func<TContext, bool> condition)
        {
            exits.Add(new JunctionExit<TContext>(probeContextName, condition, pipe));
        }

        bool IJunctionConfigurator<TContext>.ContinueOnNoMatch { set { this.continueOnNoMatch = value; } }

        public IEnumerable<ValidationResult> Validate()
        {
            if (exits == null)
                yield return this.Failure("exits", "Cannot be null");
            else
            {
                foreach (var exit in exits)
                {
                    if (exit.Condition == null)
                        yield return this.Failure("condition", "Cannot be null");

                    if (exit.Pipe == null)
                        yield return this.Failure("pipe", "Cannot be null");
                }
            }
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new JunctionFilter<TContext>(exits, continueOnNoMatch));
        }
    }
}
