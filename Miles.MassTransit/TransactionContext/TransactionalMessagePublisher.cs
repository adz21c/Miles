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
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.MessageDispatch;
using Miles.Messaging;
using Miles.Persistence;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.TransactionContext
{
    /// <summary>
    /// Dispatches events and commands on transaction commit. Stores messages and events
    /// within a data store, subject to the transaction, with consistant message identifiers to aid
    /// in message de-duplication.
    /// </summary>
    /// <seealso cref="Messaging.IEventPublisher" />
    /// <seealso cref="Messaging.ICommandPublisher" />
    public class TransactionalMessagePublisher : IEventPublisher, ICommandPublisher
    {
        private readonly IOutgoingMessageRepository outgoingMessageRepository;
        private readonly ITime time;

        // State
        private List<OutgoingMessageForDispatch> pendingSaveMessages = new List<OutgoingMessageForDispatch>();
        private List<OutgoingMessageForDispatch> pendingDispatchMessages = new List<OutgoingMessageForDispatch>();
        private readonly IActivityContext activityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalMessagePublisher" /> class.
        /// </summary>
        /// <param name="transactionContext">The transaction context.</param>
        /// <param name="outgoingMessageRepository">The outgoing message repository.</param>
        /// <param name="time">The time service.</param>
        /// <param name="activityContext">The activity context.</param>
        /// <param name="messageDispatchProcess">The message dispatch process.</param>
        public TransactionalMessagePublisher(
            ITransactionContext transactionContext,
            IOutgoingMessageRepository outgoingMessageRepository,
            ITime time,
            IActivityContext activityContext,
            IMessageDispatchProcess messageDispatchProcess)
        {
            this.outgoingMessageRepository = outgoingMessageRepository;
            this.time = time;
            this.activityContext = activityContext;

            transactionContext.PreCommit.Register(async (s, e) =>
            {
                var processingMessages = pendingSaveMessages;
                pendingSaveMessages = new List<OutgoingMessageForDispatch>();

                // Just before commit save all the outgoing messages and their ids were already generated - for consistency.
                var currentTime = time.Now;
                var outgoingMessages = processingMessages.Select(x => new OutgoingMessage(
                    x.MessageId,
                    x.CorrelationId,
                    x.MessageType.FullName,
                    x.ConceptType,
                    JsonConvert.SerializeObject(x.MessageObject),
                    currentTime));
                await outgoingMessageRepository.SaveAsync(outgoingMessages).ConfigureAwait(false);

                // Transition messages ready for dispatch
                pendingDispatchMessages.AddRange(processingMessages);
            });

            transactionContext.PostCommit.Register(async (s, e) =>
            {
                // relinquish control of the collection, let the dispatcher process own it
                var messagesForDispatch = pendingDispatchMessages;
                pendingDispatchMessages = new List<OutgoingMessageForDispatch>();

                await messageDispatchProcess.ExecuteAsync(messagesForDispatch).ConfigureAwait(false);
            });
        }

        #region IEventPublisher

        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            pendingSaveMessages.Add(new OutgoingMessageForDispatch(OutgoingMessageConceptType.Event, typeof(TEvent), evt, NewId.NextGuid(), activityContext.CorrelationId));
        }

        #endregion

        #region ICommandPublisher

        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            pendingSaveMessages.Add(new OutgoingMessageForDispatch(OutgoingMessageConceptType.Command, typeof(TCommand), cmd, NewId.NextGuid(), activityContext.CorrelationId));
        }

        #endregion
    }
}
