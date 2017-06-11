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
using System;
using System.Threading.Tasks;

namespace Miles.EventStore
{
    static class EventConnectionExtensions
    {
        public static async Task ForEachSliceStreamEventsForwardAsync(
            this IEventStoreConnection connection,
            string stream,
            int startPosition,
            int chunkSize,
            bool resolveLinkTos,
            Func<StreamEventsSlice, Task> callback)
        {
            var readSliceTask = connection.ReadStreamEventsForwardAsync(stream, startPosition, chunkSize, resolveLinkTos);

            do
            {
                var slice = await readSliceTask.ConfigureAwait(false);

                if (slice.IsEndOfStream)
                    readSliceTask = null;
                else
                    readSliceTask = connection.ReadStreamEventsForwardAsync(stream, slice.LastEventNumber, chunkSize, resolveLinkTos);

                await callback(slice).ConfigureAwait(false);
            }
            while (readSliceTask != null);
        }

        public static async Task ForEachSliceStreamEventsForwardAsync(
            this IEventStoreConnection connection,
            string stream,
            int startPosition,
            int chunkSize,
            bool resolveLinkTos,
            Action<StreamEventsSlice> callback)
        {
            var readSliceTask = connection.ReadStreamEventsForwardAsync(stream, startPosition, chunkSize, resolveLinkTos);

            do
            {
                var slice = await readSliceTask.ConfigureAwait(false);

                if (slice.IsEndOfStream)
                    readSliceTask = null;
                else
                    readSliceTask = connection.ReadStreamEventsForwardAsync(stream, slice.LastEventNumber, chunkSize, resolveLinkTos);

                callback(slice);
            }
            while (readSliceTask != null);
        }

        public static Task ForEachStreamEventsForwardAsync(
            this IEventStoreConnection connection,
            string stream,
            int startPosition,
            int chunkSize,
            bool resolveLinkTos,
            Func<ResolvedEvent, Task> callback)
        {
            return connection.ForEachSliceStreamEventsForwardAsync(
                stream,
                startPosition,
                chunkSize,
                resolveLinkTos,
                async slice =>
                {
                    foreach (var @event in slice.Events)
                        await callback(@event).ConfigureAwait(false);
                });
        }

        public static Task ForEachStreamEventsForwardAsync(
            this IEventStoreConnection connection,
            string stream,
            int startPosition,
            int chunkSize,
            bool resolveLinkTos,
            Action<ResolvedEvent> callback)
        {
            return connection.ForEachSliceStreamEventsForwardAsync(
                stream,
                startPosition,
                chunkSize,
                resolveLinkTos,
                slice =>
                {
                    foreach (var @event in slice.Events)
                        callback(@event);
                });
        }
    }
}
