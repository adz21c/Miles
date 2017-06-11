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

namespace Miles.Aggregates
{
    public abstract class Aggregate<TState, TId> : IEventSourcedAggregate<TId>, ISetableAggregateState<TState, TId>
        where TState : class, IState<TId>, IAppliesEvent, new()
    {
        private readonly List<object> newEvents = new List<object>();
        private TState state = new TState();

        public TId Id => state.Id;
        protected TState State => state;

        protected void ApplyNewEvent<TEvent>(TEvent @event) where TEvent : class
        {
            var eventApplyable = state as IAppliesEvent<TEvent>;
            if (eventApplyable == null)
                throw new UnsupportedEventTypeException(this.GetType(), typeof(TEvent));

            eventApplyable.ApplyEvent(@event);
            newEvents.Add(@event);
        }

        IEnumerable<object> IAggregate<TId>.NewEvents => newEvents;

        void IAggregate<TId>.NewEventsPublished()
        {
            newEvents.Clear();
        }

        void ISetableAggregateState<TState, TId>.SetState(TState state)
        {
            this.state = state;
        }

        long? IEventSourcedAggregate<TId>.Version { get; set; }
    }
}
