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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Miles.EventStore
{
    [Serializable]
    public class ClashingEventAliasException : Exception
    {
        public ClashingEventAliasException() : base("Event types with clashing aliases identified while processing aggregate state")
        { }

        public ClashingEventAliasException(string message) : base(message)
        { }

        public ClashingEventAliasException(string message, Exception innerException) : base(message, innerException)
        { }

        public ClashingEventAliasException(Type aggregateStateType, List<IGrouping<string, Type>> clashingAliasCheck)
            : base($"Event types with clashing aliases identified while processing aggregate state: {aggregateStateType.FullName}")
        {
            this.AggregateStateType = aggregateStateType;
            this.ClashingAliasCheck = clashingAliasCheck;
        }

        protected ClashingEventAliasException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public IEnumerable<IGrouping<string, Type>> ClashingAliasCheck { get; private set; }
        public Type AggregateStateType { get; private set; }
    }
}