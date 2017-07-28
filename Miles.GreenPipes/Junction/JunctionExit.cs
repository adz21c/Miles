using GreenPipes;
using System;

namespace Miles.GreenPipes.Junction
{
    public class JunctionExit<TContext> where TContext : class, PipeContext
    {
        public JunctionExit(string description, Func<TContext, bool> condition, IPipe<TContext> pipe)
        {
            this.ConditionDescription = description;
            this.Condition = condition;
            this.Pipe = pipe;
        }

        public string ConditionDescription { get; private set; }

        public Func<TContext, bool> Condition { get; private set; }

        public IPipe<TContext> Pipe { get; private set; }
    }
}
