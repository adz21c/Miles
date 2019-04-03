using GreenPipes;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.GreenPipes.ContainerScope
{
    public class ContainerScopeContextImp : BasePipeContext, ContainerScopeContext
    {
        public ContainerScopeContextImp(IScopedServiceLocator serviceLocator, PipeContext context) : base(context)
        {
            this.ServiceLocator = serviceLocator;
        }

        public ContainerScopeContextImp(PipeContext context) : base(context)
        { }

        public IScopedServiceLocator ServiceLocator { get; private set; }

        internal void AssignContainer(IScopedServiceLocator serviceLocator)
        {
            Debug.Assert(serviceLocator != null, "serviceLocator != null");
            if (this.ServiceLocator != null)
                throw new InvalidOperationException("Service locagtor already assigned");

            this.ServiceLocator = serviceLocator;
        }
    }
}
