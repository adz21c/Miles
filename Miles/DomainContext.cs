﻿/*
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
using Miles.Messaging;

namespace Miles
{
    /// <summary>
    /// Convenience type of services commonly needed by any domain process.
    /// </summary>
    public class DomainContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainContext"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public DomainContext(ITime time, IEventPublisher eventPublisher)
        {
            this.Time = time;
            this.EventPublisher = eventPublisher;
        }

        /// <summary>
        /// Gets the time service.
        /// </summary>
        /// <value>
        /// The time service.
        /// </value>
        public ITime Time { get; private set; }

        /// <summary>
        /// Gets the event publisher service.
        /// </summary>
        /// <value>
        /// The event publisher service.
        /// </value>
        public IEventPublisher EventPublisher { get; private set; }
    }
}
