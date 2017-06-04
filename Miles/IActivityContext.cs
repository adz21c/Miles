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
using System;

namespace Miles
{
    /// <summary>
    /// Gives each piece of activity in a system a unique Id that can be stored and/or logged to allow
    /// developers to see the affect user activity has had across a whole system.
    /// </summary>
    public interface IActivityContext
    {
        /// <summary>
        /// Gets the activity identifier, an Id for an individual activity/unit.
        /// </summary>
        /// <value>
        /// The activity identifier.
        /// </value>
        Guid ActivityId { get; }

        /// <summary>
        /// Gets the correlation identifier, an Id to correlate multiple activities.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        Guid CorrelationId { get; }
    }
}
