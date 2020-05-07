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
using System.Collections.Generic;

namespace Miles.MassTransit.MessageDeduplication
{
    class MessageDeduplicationSpecification<TContext> : IPipeSpecification<TContext> where TContext : class, ConsumeContext
    {
        public const string QueueNameKey = "QueueName";
        public const string NoQueueNameError = "Cannot be null or whitespace";

        public string QueueName { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            if (string.IsNullOrWhiteSpace(QueueName))
                yield return this.Failure(QueueNameKey, QueueName, NoQueueNameError);
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new MessageDeduplicationFilter<TContext>(QueueName));
        }
    }
}
