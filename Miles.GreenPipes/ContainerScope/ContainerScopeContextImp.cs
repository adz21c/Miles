using GreenPipes;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
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

        public IScopedServiceLocator ServiceLocator { get; }
    }
}
