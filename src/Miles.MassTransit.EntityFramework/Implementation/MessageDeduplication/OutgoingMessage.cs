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
using Miles.MassTransit.MessageDispatch;
using System;

namespace Miles.MassTransit.EntityFramework.Implementation.MessageDeduplication
{
    /// <summary>
    /// Represents the outgoing message serialized for data storage.
    /// </summary>
    public class OutgoingMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessage"/> class.
        /// </summary>
        public OutgoingMessage()
        { }

        /// <summary>
        /// Gets or sets a unique identifier used for message de-duplication between endpoints.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public Guid MessageId { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the class type the message represents.
        /// </summary>
        /// <value>
        /// The name of the class type the message represents.
        /// </value>
        public string ClassTypeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the message is a Command or Event.
        /// </summary>
        /// <value>
        /// Command or Event.
        /// </value>
        public DispatchType ConceptType { get; set; }

        /// <summary>
        /// Gets or sets the serialized message.
        /// </summary>
        /// <value>
        /// The serialized message.
        /// </value>
        public string SerializedMessage { get; set; }

        /// <summary>
        /// Gets or sets when the message was created.
        /// </summary>
        /// <value>
        /// When the message was created.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets when the message was dispatched. If <c>null</c> then the message has not yet been dispatched.
        /// </summary>
        /// <value>
        /// When the message was dispatched.
        /// </value>
        public DateTime? DispatchedDate { get; set; }

        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string ResponseAddress { get; set; }
        public string FaultAddress { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? ConversationId { get; set; }
        public Guid? InitiatorId { get; set; }
        public Guid? ScheduledMessageId { get; set; }
        //public SendHeaders Headers { get; }
        public TimeSpan? TimeToLive { get; set; }
        //public ContentType ContentType { get; set; }
        public bool? Durable { get; set; }
        public bool? Mandatory { get; set; }
    }
}
