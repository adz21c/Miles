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
using System.Runtime.Serialization;

namespace Miles.Aggregates
{
    [Serializable]
    public class UnsupportedEventTypeException : Exception
    {
        public UnsupportedEventTypeException()
        {
        }

        public UnsupportedEventTypeException(string message) : base(message)
        {
        }

        public UnsupportedEventTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsupportedEventTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnsupportedEventTypeException(Type aggregateType, Type eventType) : this($"{aggregateType.FullName} doesn't processs event type {eventType.FullName}.")
        {
            this.AggregateType = aggregateType;
            this.EventType = eventType;
        }

        public Type AggregateType { get; private set; }
        public Type EventType { get; private set; }
    }
}