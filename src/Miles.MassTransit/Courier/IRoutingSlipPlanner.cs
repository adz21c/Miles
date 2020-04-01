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
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using System;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
namespace Miles.MassTransit.Courier
{
    /// <summary>
    /// Extends <see cref="global::MassTransit.Courier.ItineraryBuilder"/> to use conventions for activity addresses. 
    /// </summary>
    /// <seealso cref="global::MassTransit.Courier.ItineraryBuilder" />
    public interface IRoutingSlipPlanner : ItineraryBuilder
    {
        /// <summary>
        /// Adds an activity to the routing slip identifying the activity address by convention.
        /// </summary>
        /// <typeparam name="TArguments">.</typeparam>
        /// <param name="name">The activity name.</param>
        /// <param name="arguments">The argument values.</param>
        void AddActivity<TArguments>(string name, TArguments arguments) where TArguments : class;

        /// <summary>
        /// Adds a message subscription to the routing slip that will dispatch the specified events.
        /// </summary>
        /// <typeparam name="TEvent">Custom event type. Identifies the types exchange by convention.</typeparam>
        /// <param name="events">The events to subscribe against.</param>
        /// <param name="callback">Callback to send custom event types.</param>
        /// <returns></returns>
        Task AddSubscription<TEvent>(RoutingSlipEvents events, Func<ISendEndpoint, Task> callback) where TEvent : class;

        /// <summary>
        /// Adds a message subscription to the routing slip that will dispatch the specified events.
        /// </summary>
        /// <typeparam name="TEvent">Custom event type. Identifies the types exchange by convention.</typeparam>
        /// <param name="events">The events to subscribe against.</param>
        /// <param name="contents">The contents of the routing slip to include in the events.</param>
        /// <param name="callback">Callback to send custom event types.</param>
        /// <returns></returns>
        Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, Func<ISendEndpoint, Task> callback) where TEvent : class;

        /// <summary>
        /// Adds a message subscription to the routing slip that will dispatch the specified events.
        /// </summary>
        /// <typeparam name="TEvent">Custom event type. Identifies the types exchange by convention.</typeparam>
        /// <param name="events">The events to subscribe against.</param>
        /// <param name="contents">The contents of the routing slip to include in the events.</param>
        /// <param name="activityName">Only send events for the specified activity.</param>
        /// <param name="callback">Callback to send custom event types.</param>
        /// <returns></returns>
        Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, string activityName, Func<ISendEndpoint, Task> callback) where TEvent : class;

        RoutingSlip Build();
    }
}