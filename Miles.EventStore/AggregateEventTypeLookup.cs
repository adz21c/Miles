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
using Miles.Aggregates;
using Miles.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miles.EventStore
{
    public class AggregateEventTypeLookup<TAggregate, TState> : IAggregateEventTypeLookup<TAggregate> where TState : class, IAppliesEvent
    {
        private static readonly Type iAppliesEvents = typeof(IAppliesEvent<>);

        private readonly Dictionary<string, Type> aliasToType;
        private readonly Dictionary<Type, string> typeToAlias;

        public AggregateEventTypeLookup()
        {
            var applyableEvents = typeof(TState).GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == iAppliesEvents)
                .Select(t => t.GetGenericArguments().First());

            var nameAndType = applyableEvents
                .Select(et => new
                {
                    Alias = et.GetCustomAttribute<EventNameAttribute>(false)?.Name ?? et.Name,
                    Type = et
                })
                .ToList();

            var aliasAndType = applyableEvents
                .SelectMany(et => et.GetCustomAttributes<EventAliasAttribute>(false)
                    .Select(n => new
                    {
                        Alias = n.Alias,
                        Type = et
                    }))
                    .Concat(nameAndType)
                    .ToList();

            var clashingAliasCheck = aliasAndType.GroupBy(x => x.Alias, x => x.Type).Where(x => x.Count() > 1).ToList();
            if (clashingAliasCheck.Any())
                throw new ClashingEventAliasException(typeof(TState), clashingAliasCheck);

            aliasToType = aliasAndType.ToDictionary(x => x.Alias, x => x.Type);
            typeToAlias = nameAndType.ToDictionary(x => x.Type, x => x.Alias);
        }

        public Type lookupType(string alias)
        {
            Type et;
            if (aliasToType.TryGetValue(alias, out et))
                return et;

            return null;
        }

        public string lookupName(Type eventType)
        {
            string alias;
            if (typeToAlias.TryGetValue(eventType, out alias))
                return alias;

            return null;
        }
    }
}
