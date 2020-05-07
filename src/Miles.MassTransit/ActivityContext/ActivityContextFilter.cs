/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
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
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.ActivityContext
{
    class ActivityContextFilter<TContext> : IFilter<TContext> where TContext : class, ConsumeContext
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("activity-context");
        }

        [DebuggerNonUserCode]
        public Task Send(TContext context, IPipe<TContext> next)
        {
            var serviceProvider = context.GetPayload<IServiceProvider>();
            var activityContext = serviceProvider.GetRequiredService<ActivityContext>();
            activityContext.ActivityId = context.MessageId ?? context.RequestId ?? NewId.NextGuid();
            activityContext.CorrelationId = context.CorrelationId ?? NewId.NextGuid();

            return next.Send(context);
        }
    }
}
