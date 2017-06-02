/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
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
using Microsoft.Practices.Unity;
using System;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Configuration options for Unity Miles registration
    /// </summary>
    public class UnityRegistrationConfiguration
    {
        /// <summary>
        /// Callback that creates the lifetime manager used for child container isolation.
        /// </summary>
        /// <value>
        /// The child container lifetime manager factory.
        /// </value>
        public Func<Type, LifetimeManager> ChildContainerLifetimeManagerFactory { get; set; } = t => new HierarchicalLifetimeManager();

        /// <summary>
        /// Gets or sets the dispatcher to use for commands.
        /// </summary>
        /// <value>
        /// The command dispatcher.
        /// </value>
        public CommandDispatcherTypes CommandDispatcher { get; set; } = CommandDispatcherTypes.Publish;
    }
}
