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
using System.Linq.Expressions;

namespace Miles.EventStore
{
    public class AggregateBuilder<TAggregate, TId, TState> : IAggregateBuilder<TAggregate>
        where TAggregate : ISetableAggregateState<TState, TId>, IEventSourcedAggregate<TId>, new()
        where TState : class, IState<TId>, IAppliesEvent, new()
    {
        private readonly ISerializer<TAggregate> serializer;
        private readonly IAggregateEventTypeLookup<TAggregate> eventTypeLookup;

        private long version = 0;
        private readonly TState state = new TState();

        public AggregateBuilder(
            ISerializer<TAggregate> serializer,
            IAggregateEventTypeLookup<TAggregate> eventTypeLookup)
        {
            this.serializer = serializer;
            this.eventTypeLookup = eventTypeLookup;
        }

        public void AddEvent(RecordedEvent @event)
        {
            var eventType = eventTypeLookup.lookupType(@event.EventType);
            var eventObj = serializer.DeSerialize(@event.Data, eventType);

            var proxyMethod = ProxyMe(eventType);
            proxyMethod(state, eventObj);
            version = @event.EventNumber;
        }

        public TAggregate Build()
        {
            var aggregate = new TAggregate() { Version = version };
            aggregate.SetState(state);
            return aggregate;
        }

        private Action<object, object> ProxyMe(Type eventType)
        {
            // TODO: Build on app start and make this a lazy lookup
            var appliesEventType = typeof(IAppliesEvent<>).MakeGenericType(eventType);
            var method = appliesEventType.GetMethod("ApplyEvent");
            var stateParam = Expression.Parameter(typeof(object), "state");
            var eventParam = Expression.Parameter(typeof(object), "@event");
            var proxyMethod = Expression.Call(Expression.TypeAs(stateParam, appliesEventType), method, Expression.TypeAs(eventParam, eventType));
            return Expression.Lambda<Action<object, object>>(proxyMethod, stateParam, eventParam).Compile();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AggregateBuilder() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
