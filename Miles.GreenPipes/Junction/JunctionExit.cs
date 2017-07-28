using GreenPipes;
using System;

namespace Miles.GreenPipes.Junction
{
    public class JunctionExit<TContext> where TContext : class, PipeContext
    {
        public JunctionExit(string probeContextName, Func<TContext, bool> condition, IPipe<TContext> pipe)
        {
            this.ProbeContextName = probeContextName;
            this.Condition = condition;
            this.Pipe = pipe;
        }

        public string ProbeContextName { get; private set; }

        public Func<TContext, bool> Condition { get; private set; }

        public IPipe<TContext> Pipe { get; private set; }
    }
}
