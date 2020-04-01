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
using Miles.MassTransit.Courier;
using System;
using System.Collections.Generic;

namespace MassTransit.Courier
{
    public static class ItineraryBuilderExtensions
    {
        /// <summary>
        /// Adds an activity to the routing slip specifying execution address by convention.
        /// </summary>
        /// <typeparam name="TArguments">Arguements DTO</typeparam>
        /// <param name="builder"></param>
        /// <param name="name">The activity name.</param>
        /// <param name="hostAddress">The activity host address.</param>
        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()));
        }

        /// <summary>
        /// Adds an activity to the routing slip specifying activity arguments and execution address by convention.
        /// </summary>
        /// <typeparam name="TArguments">Arguements DTO</typeparam>
        /// <param name="builder"></param>
        /// <param name="name">The activity name.</param>
        /// <param name="hostAddress">The activity host address.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress, object arguments)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()), arguments);
        }

        /// <summary>
        /// Adds an activity to the routing slip specifying activity arguments and execution address by convention.
        /// </summary>
        /// <typeparam name="TArguments">Arguements DTO</typeparam>
        /// <param name="builder"></param>
        /// <param name="name">The activity name.</param>
        /// <param name="hostAddress">The activity host address.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress, IDictionary<string, object> arguments)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()), arguments);
        }

        /// <summary>
        /// Adds an activity to the routing slip specifying activity arguments and execution address by convention.
        /// </summary>
        /// <typeparam name="TArguments">Arguements DTO</typeparam>
        /// <param name="builder"></param>
        /// <param name="name">The activity name.</param>
        /// <param name="hostAddress">The activity host address.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress, TArguments arguments)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()), arguments);
        }
    }
}
