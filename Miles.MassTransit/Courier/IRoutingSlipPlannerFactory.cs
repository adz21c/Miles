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
using MassTransit.Courier.Contracts;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Courier
{
    /// <summary>
    /// Factory designed to be injected to ease the creation of <see cref="IExecutableRoutingSlipPlanner"/>.
    /// </summary>
    public interface IRoutingSlipPlannerFactory
    {
        IRoutingSlipPlanner Create(Guid trackingNumber);

        IRoutingSlipPlanner Create(RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector);

        IRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs);

        IRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary);
    }
}
