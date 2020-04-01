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
using System;

namespace Miles.MassTransit
{
    /// <summary>
    /// MassTransit implementation that uses the consumer context correlation identifier if available.
    /// </summary>
    /// <seealso cref="Miles.IActivityContext" />
    public class ConsumerActivityContext : IActivityContext
    {
        public ConsumerActivityContext(ConsumeContext consumeContext)
        {
            ActivityId = consumeContext.MessageId ?? consumeContext.RequestId ?? Guid.Empty;
            CorrelationId = consumeContext.CorrelationId ?? Guid.Empty;
        }

        public Guid ActivityId { get; private set; }

        public Guid CorrelationId { get; private set; }
    }
}
