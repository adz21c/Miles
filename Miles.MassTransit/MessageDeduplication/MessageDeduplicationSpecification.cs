﻿/*
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
using System.Linq;

namespace Miles.MassTransit.MessageDeduplication
{
    class MessageDeduplicationSpecification<TContext> : IPipeSpecification<TContext> where TContext : class, ConsumeContext
    {
        private MessageDeduplicationConfigurator config;

        public MessageDeduplicationSpecification(MessageDeduplicationConfigurator config)
        {
            this.config = config;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new MessageDeduplicationFilter<TContext>(config.QueueName));
        }
    }
}
