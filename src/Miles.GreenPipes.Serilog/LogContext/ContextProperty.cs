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

namespace Miles.GreenPipes.Serilog.LogContext
{
    /// <summary>
    /// DTO to hold all the details required for the calls to <see cref="global::Serilog.Context.LogContext"/>.
    /// </summary>
    /// <typeparam name="TContext">MassTransit context type</typeparam>
    class ContextProperty<TContext> where TContext : class, PipeContext
    {
        public ContextProperty(string propertyName, Func<TContext, object> propertyAccessor, bool destructureObjects)
        {
            this.PropertyName = propertyName;
            this.PropertyAccessor = propertyAccessor;
            this.DestructureObjects = destructureObjects;
        }

        public string PropertyName { get; private set; }

        public Func<TContext, object> PropertyAccessor { get; private set; }

        public bool DestructureObjects { get; private set; }
    }
}
