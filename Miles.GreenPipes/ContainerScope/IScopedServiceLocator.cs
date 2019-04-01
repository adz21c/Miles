using GreenPipes;
using Microsoft.Practices.ServiceLocation;
using System;

namespace Miles.GreenPipes.ContainerScope
{
    public interface IScopedServiceLocator : IServiceLocator, IDisposable
    {
        string ContainerType { get; }

        IScopedServiceLocator CreateChildScope();
    }
}