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
using EventStore.ClientAPI;
using Miles.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.EventStore
{
    public class AggregateManager<TAggregate, TId, TState> : IAggregateManager<TAggregate, TId>
        where TAggregate : class, IEventSourcedAggregate<TId>, ISetableAggregateState<TState, TId>, new()
        where TState : class, IState<TId>, IAppliesEvent, new()
    {
        private readonly ISerializer<TAggregate> serializer;
        private readonly IAggregateEventTypeLookup<TAggregate> eventTypeLookup;

        public AggregateManager(
            ISerializer<TAggregate> serializer,
            IAggregateEventTypeLookup<TAggregate> eventTypeLookup)
        {
            this.serializer = serializer;
            this.eventTypeLookup = eventTypeLookup;
        }


        public IAggregateBuilder<TAggregate> CreateBuilder(TAggregate aggregate = default(TAggregate))
        {
            return new AggregateBuilder<TAggregate, TId, TState>(serializer, eventTypeLookup);
        }

        public EventData[] CreateEventData(IEnumerable<object> newEvents)
        {
            var results = new List<EventData>(newEvents.Count());

            foreach (var newEvent in newEvents)
            {
                var eventType = newEvent.GetType();
                var isJson = serializer.IsJson(eventType);
                var eventTypeName = eventTypeLookup.lookupName(eventType);
                var serializedEvent = serializer.Serialize(newEvent);
                var serializedMetadata = serializer.Serialize(new object());
                var eventData = new EventData(Guid.NewGuid(), eventTypeName, isJson, serializedEvent, serializedMetadata);
                results.Add(eventData);
            }

            return results.ToArray();
        }
    }
}
