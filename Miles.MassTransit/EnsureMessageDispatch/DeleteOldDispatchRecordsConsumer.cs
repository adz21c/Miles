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
using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    /// <summary>
    /// Built in consumer to perform regular clean up of old messages.
    /// </summary>
    /// <remarks>
    /// This doesn't need to mean deleting, it could mean archiving. The aim is to keep the data lean for fast processing.
    /// </remarks>
    class DeleteOldDispatchRecordsConsumer : IConsumer<IDeleteOldDispatchRecordsCommand>
    {
        private readonly IDispatchedRepository dispatchedRepository;

        public DeleteOldDispatchRecordsConsumer(IDispatchedRepository dispatchedRepository)
        {
            this.dispatchedRepository = dispatchedRepository;
        }

        public Task Consume(ConsumeContext<IDeleteOldDispatchRecordsCommand> context)
        {
            return dispatchedRepository.DeleteOldRecordsAsync();
        }
    }
}