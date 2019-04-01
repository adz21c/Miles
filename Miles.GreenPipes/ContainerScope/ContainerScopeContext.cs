using GreenPipes;

namespace Miles.GreenPipes.ContainerScope
{
    public interface ContainerScopeContext : PipeContext
    {
        IScopedServiceLocator ServiceLocator { get; }
    }
}
