using GreenPipes;
using Miles.DependencyInjection;
using System;
using System.Diagnostics;

namespace Miles.GreenPipes.ContainerScope
{
    public class ContainerScopeContextImp : BasePipeContext, ContainerScopeContext
    {
        public ContainerScopeContextImp(IContainer container, PipeContext context) : base(context)
        {
            this.Container = container;
        }

        public ContainerScopeContextImp(PipeContext context) : base(context)
        { }

        public IContainer Container { get; private set; }

        internal void AssignContainer(IContainer container)
        {
            Debug.Assert(container != null, "container != null");
            if (this.Container != null)
                throw new InvalidOperationException("Container already assigned");

            this.Container = container;
        }
    }
}
