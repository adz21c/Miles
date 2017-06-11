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
using Newtonsoft.Json;
using System;
using System.Text;

namespace Miles.EventStore.NewtonsoftJson
{
    public class NewtonsoftJsonSerializer<TAggregate> : ISerializer<TAggregate>
    {
        private readonly JsonSerializerSettings settings;

        public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public bool IsJson(Type eventType)
        {
            return true;
        }

        public byte[] Serialize(object @event)
        {
            var text = JsonConvert.SerializeObject(@event, settings);
            return Encoding.UTF8.GetBytes(text);
        }

        public object DeSerialize(byte[] @event, Type eventType)
        {
            var text = Encoding.UTF8.GetString(@event);
            return JsonConvert.DeserializeObject(text, eventType, settings);
        }
    }
}
