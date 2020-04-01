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
using Miles.MassTransit.MessageDeduplication;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.Implementation.MessageDeduplication
{
    public class ConsumptionRecorder<TContext> : IConsumptionRecorder where TContext : DbContext
    {
        private readonly TContext dbContext;

        public ConsumptionRecorder(TContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> RecordAsync(MessageContext context, string queueName)
        {
            dbContext.Set<IncomingMessage>().Add(new IncomingMessage(context.MessageId.Value, queueName, DateTime.Now));

            try
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
