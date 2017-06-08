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

using Miles.GreenPipes.Serilog.LogContext;
using System;

namespace GreenPipes
{
    public static class LogContextExtensions
    {
        /// <summary>
        /// Configures a <see cref="global::Serilog.Context.LogContext"/> around the pipe.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure"></param>
        public static void UseLogContext<TContext>(this IPipeConfigurator<TContext> configurator, Action<ILogContextConfigurator<TContext>> configure = null)
            where TContext : class, PipeContext
        {
            var spec = new LogContextSpecification<TContext>();
            configure?.Invoke(spec);

            configurator.AddPipeSpecification(spec);
        }
    }
}
