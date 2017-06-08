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

namespace Miles.GreenPipes.ContainerScope
{
    /// <summary>
    /// Abstracts the creation of an initial <see cref="IContainerStack"/> to allow for multiple
    /// container implementations.
    /// </summary>
    public interface IContainerStackFactory : ISpecification
    {
        /// <summary>
        /// Returns the name of the container implementation for debugging/probe purposes.
        /// </summary>
        string ContainerType { get; }

        /// <summary>
        /// Creates the <see cref="IContainerStack"/>. The context is supplied to allow for child container registrations.
        /// </summary>
        /// <typeparam name="TContext">MassTrasit context type</typeparam>
        /// <param name="context">MassTrasit context</param>
        /// <returns>The new instance</returns>
        IContainerStack Create<TContext>(TContext context) where TContext : class, PipeContext;
    }
}
