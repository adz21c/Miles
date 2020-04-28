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
using MassTransit;
//using MassTransit.Courier.Contracts;
//using Miles.MassTransit.Courier;
using Miles.MassTransit.MessageDispatch;
using Miles.Messaging;
using Miles.Persistence;
using System.Collections.Generic;

namespace Miles.MassTransit.TransactionContext
{
    /// <summary>
    /// Dispatches events and commands on transaction commit. Stores messages and events
    /// within a data store, subject to the transaction, with consistant message identifiers to aid
    /// in message de-duplication.
    /// </summary>
    /// <seealso cref="Messaging.IEventPublisher" />
    /// <seealso cref="Messaging.ICommandPublisher" />
    public class TransactionalMessagePublisher : IEventPublisher, ICommandPublisher//, IRoutingSlipPublisher
    {
        private readonly IOutgoingMessageRepository outgoingMessageRepository;

        // State
        private List<OutgoingMessageForDispatch> pendingSaveMessages = new List<OutgoingMessageForDispatch>();
        private List<OutgoingMessageForDispatch> pendingDispatchMessages = new List<OutgoingMessageForDispatch>();
        private readonly IActivityContext activityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalMessagePublisher" /> class.
        /// </summary>
        /// <param name="transactionContext">The transaction context.</param>
        /// <param name="outgoingMessageRepository">The outgoing message repository.</param>
        /// <param name="activityContext">The activity context.</param>
        /// <param name="messageDispatchProcess">The message dispatch process.</param>
        public TransactionalMessagePublisher(
            ITransactionContext transactionContext,
            IOutgoingMessageRepository outgoingMessageRepository,
            IActivityContext activityContext,
            IMessageDispatchProcess messageDispatchProcess)
        {
            this.outgoingMessageRepository = outgoingMessageRepository;
            this.activityContext = activityContext;

            transactionContext.PreCommit.Register(async (s, e) =>
            {
                var processingMessages = pendingSaveMessages;
                pendingSaveMessages = new List<OutgoingMessageForDispatch>();

                // Just before commit save all the outgoing messages and their ids were already generated - for consistency.
                await outgoingMessageRepository.SaveAsync(processingMessages).ConfigureAwait(false);

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
            pendingSaveMessages.Add(new OutgoingMessageForDispatch(DispatchType.Publish, typeof(TEvent), evt, NewId.NextGuid(), activityContext.CorrelationId));
        }

        #endregion

        #region ICommandPublisher

        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            pendingSaveMessages.Add(new OutgoingMessageForDispatch(DispatchType.Publish, typeof(TCommand), cmd, NewId.NextGuid(), activityContext.CorrelationId));
        }

        #endregion

        //#region IRoutingSlipPublisher

        //void IRoutingSlipPublisher.Publish(RoutingSlip slip)
        //{
        //    pendingSaveMessages.Add(new OutgoingMessageForDispatch(DispatchType.RoutingSlip, typeof(RoutingSlip), slip, NewId.NextGuid(), activityContext.CorrelationId));
        //}

        //#endregion
    }
}
