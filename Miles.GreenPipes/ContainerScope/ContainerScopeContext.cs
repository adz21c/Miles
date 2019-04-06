using GreenPipes;
using Miles.DependencyInjection;

namespace Miles.GreenPipes.ContainerScope
{
    public interface ContainerScopeContext : PipeContext
    {
        IContainer Container { get; }
    }
}
