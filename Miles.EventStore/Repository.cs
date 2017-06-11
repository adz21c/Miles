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
using Miles.Persistence;
using System;
using System.Threading.Tasks;

namespace Miles.EventStore
{
    public class Repository<TAggregate, TId> : IRepository<TAggregate, TId> where TAggregate : class, IEventSourcedAggregate<TId>
    {
        private static readonly Type AggregateType = typeof(TAggregate);

        private readonly IEventStoreConnection connection;
        private readonly IAggregateManager<TAggregate, TId> aggregateManager;
        private readonly IStreamIdGenerator streamIdGenerator;

        public Repository(
            IEventStoreConnection connection,
            IAggregateManager<TAggregate, TId> aggregateManager,
            IStreamIdGenerator streamIdGenerator)
        {
            this.connection = connection;
            this.aggregateManager = aggregateManager;
            this.streamIdGenerator = streamIdGenerator;
        }

        public async Task<TAggregate> GetByIdAsync(TId id)
        {
            var streamId = streamIdGenerator.GenerateStreamId(AggregateType, id);
            using (var builder = aggregateManager.CreateBuilder())
            {
                await connection.ForEachStreamEventsForwardAsync(streamId, 0, 100, false, e =>
                {
                    builder.AddEvent(e.Event);
                });

                return builder.Build();
            }
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            var streamId = streamIdGenerator.GenerateStreamId(AggregateType, aggregate.Id);

            var eventData = aggregateManager.CreateEventData(aggregate.NewEvents);
            var expectedVersion = aggregate.Version ?? ExpectedVersion.NoStream;

            var result = await connection.AppendToStreamAsync(streamId, expectedVersion, eventData);
            aggregate.NewEventsPublished();
            aggregate.Version = result.NextExpectedVersion;
        }
    }
}
