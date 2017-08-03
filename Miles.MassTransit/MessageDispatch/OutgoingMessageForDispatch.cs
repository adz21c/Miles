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
using System;

namespace Miles.MassTransit.MessageDispatch
{
    /// <summary>
    /// Represents a message in memory awaiting dispatch.
    /// </summary>
    public class OutgoingMessageForDispatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessageForDispatch" /> class.
        /// </summary>
        /// <param name="dispatchType">Command or Event.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageObject">The message object.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        public OutgoingMessageForDispatch(
            DispatchType dispatchType,
            Type messageType,
            Object messageObject,
            Guid messageId,
            Guid correlationId)
        {
            this.DispatchType = dispatchType;
            this.MessageType = messageType;
            this.MessageObject = messageObject;
            this.MessageId = messageId;
            this.CorrelationId = correlationId;
        }

        /// <summary>
        /// Gets a value indicating if the message is a Command or Event.
        /// </summary>
        /// <value>
        /// Command or Event.
        /// </value>
        public DispatchType DispatchType { get; private set; }

        /// <summary>
        /// Gets the type of the message as indicated by the publish call.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public Type MessageType { get; private set; }

        /// <summary>
        /// Gets the message object.
        /// </summary>
        /// <value>
        /// The message object.
        /// </value>
        public Object MessageObject { get; private set; }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public Guid CorrelationId { get; private set; }

        public Uri SourceAddress { get; set; }
        public Uri DestinationAddress { get; set; }
        public Uri ResponseAddress { get; set; }
        public Uri FaultAddress { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? ConversationId { get; set; }
        public Guid? InitiatorId { get; set; }
        public Guid? ScheduledMessageId { get; set; }
        //public SendHeaders Headers { get; }
        public TimeSpan? TimeToLive { get; set; }
        //public ContentType ContentType { get; set; }
        public bool? Durable { get; set; }
        public bool? Mandatory { get; set; }

        public void Apply(PublishContext context)
        {
            context.MessageId = this.MessageId;
            context.CorrelationId = this.CorrelationId;
            if (this.SourceAddress != null)
                context.SourceAddress = this.SourceAddress;
            if (this.DestinationAddress != null)
                context.DestinationAddress = this.DestinationAddress;
            if (this.ResponseAddress != null)
                context.ResponseAddress = this.ResponseAddress;
            if (this.FaultAddress != null)
                context.FaultAddress = this.FaultAddress;
            if (this.RequestId.HasValue)
                context.RequestId = this.RequestId;
            if (this.ConversationId.HasValue)
                context.ConversationId = this.ConversationId;
            if (this.InitiatorId.HasValue)
                context.InitiatorId = this.InitiatorId;
            if (this.ScheduledMessageId.HasValue)
                context.ScheduledMessageId = this.ScheduledMessageId;
            if (this.TimeToLive.HasValue)
                context.TimeToLive = this.TimeToLive;
            if (this.Durable.HasValue)
                context.Durable = this.Durable.Value;
            if (this.Mandatory.HasValue)
                context.Mandatory = this.Mandatory.Value;
        }
    }
}
