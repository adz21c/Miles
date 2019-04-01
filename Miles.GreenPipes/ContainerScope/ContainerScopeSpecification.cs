/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
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
using System.Collections.Generic;

namespace Miles.GreenPipes.ContainerScope
{
    public class ContainerScopeSpecification<TContext> : IPipeSpecification<TContext> where TContext : class, PipeContext
    {
        private readonly IScopedServiceLocator _rootServiceLocator;

        public ContainerScopeSpecification(IScopedServiceLocator rootServiceLocator)
        {
            _rootServiceLocator = rootServiceLocator;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            //if (containerStackFactory == null)
            //    yield return this.Warning("containerStackFactory", "Must have a container scope configured before this call that has a factory");
            //else
            //    foreach (var result in containerStackFactory.Validate())
            //        yield return result;
            return new ValidationResult[] { };
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new ContainerScopeFilter<TContext>(_rootServiceLocator));
        }
    }
}
