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
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.MessageDispatch;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.Implementation.MessageDeduplication
{
    public class OutgoingMessageRepository : IOutgoingMessageRepository
    {
        private readonly DbContext dbContext;
        private readonly ITime time;

        public OutgoingMessageRepository(DbContext dbContext, ITime time)
        {
            this.dbContext = dbContext;
            this.time = time;
        }

        public async Task SaveAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            var currentTime = time.Now;
            dbContext.Set<OutgoingMessage>().AddRange(
                messages.Select(x => new OutgoingMessage
                {
                    MessageId = x.MessageId,
                    CorrelationId = x.CorrelationId,
                    ClassTypeName = x.MessageType.FullName,
                    ConceptType = x.ConceptType,
                    SerializedMessage = JsonConvert.SerializeObject(x.MessageObject),
                    CreatedDate = currentTime
                }));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
