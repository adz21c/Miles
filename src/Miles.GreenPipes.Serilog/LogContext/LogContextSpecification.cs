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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.GreenPipes.Serilog.LogContext
{
    class LogContextSpecification<TContext> : IPipeSpecification<TContext>, ILogContextConfigurator<TContext> where TContext : class, PipeContext
    {
        private readonly List<ContextProperty<TContext>> contextProperties = new List<ContextProperty<TContext>>();

        public IEnumerable<ValidationResult> Validate()
        {
            if (!contextProperties.Any())
                yield return this.Warning("contextProperties", "No context properties defined.");
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new LogContextFilter<TContext>(contextProperties));
        }

        void ILogContextConfigurator<TContext>.PushProperty(string name, Func<TContext, object> propertyAccessor, bool destructureObjects)
        {
            contextProperties.Add(new ContextProperty<TContext>(name, propertyAccessor, destructureObjects));
        }
    }
}
