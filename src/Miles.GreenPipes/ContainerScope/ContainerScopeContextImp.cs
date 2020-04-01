/*
 *     Copyright 2019 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using GreenPipes;
using Miles.DependencyInjection;
using System;
using System.Diagnostics;

namespace Miles.GreenPipes.ContainerScope
{
    class ContainerScopeContextImp : BasePipeContext, ContainerScopeContext
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
