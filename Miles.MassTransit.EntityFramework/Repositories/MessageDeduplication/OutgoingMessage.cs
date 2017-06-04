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

namespace Miles.MassTransit.EntityFramework.MessageDeduplication
{
    /// <summary>
    /// Represents the outgoing message serialized for data storage.
    /// </summary>
    public class OutgoingMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessage"/> class.
        /// Empty constructor is required by some ORMs and serializers.
        /// </summary>
        protected OutgoingMessage()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessage" /> class.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="classTypeName">Name of the class type.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <param name="createdDate">The date the message was created.</param>
        public OutgoingMessage(Guid messageId, Guid correlationId, string classTypeName, OutgoingMessageConceptType messageType, string serializedMessage, DateTime createdDate)
        {
            this.CreatedDate = createdDate;
            this.MessageId = messageId;
            this.CorrelationId = correlationId;
            this.ClassTypeName = classTypeName;
            this.ConceptType = messageType;
            this.SerializedMessage = serializedMessage;
        }

        /// <summary>
        /// Gets a unique identifier used for message de-duplication between endpoints.
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

        /// <summary>
        /// Gets the name of the class type the message represents.
        /// </summary>
        /// <value>
        /// The name of the class type the message represents.
        /// </value>
        public string ClassTypeName { get; private set; }

        /// <summary>
        /// Gets a value indicating if the message is a Command or Event.
        /// </summary>
        /// <value>
        /// Command or Event.
        /// </value>
        public OutgoingMessageConceptType ConceptType { get; private set; }

        /// <summary>
        /// Gets the serialized message.
        /// </summary>
        /// <value>
        /// The serialized message.
        /// </value>
        public string SerializedMessage { get; private set; }

        /// <summary>
        /// Gets when the message was created.
        /// </summary>
        /// <value>
        /// When the message was created.
        /// </value>
        public DateTime CreatedDate { get; private set; }

        /// <summary>
        /// Gets when the message was dispatched. If <c>null</c> then the message has not yet been dispatched.
        /// </summary>
        /// <value>
        /// When the message was dispatched.
        /// </value>
        public DateTime? DispatchedDate { get; private set; }

        /// <summary>
        /// Indicate the message has been dispatched.
        /// </summary>
        /// <param name="when">When the message was dispatched.</param>
        public void Dispatched(DateTime when)
        {
            DispatchedDate = when;
        }
    }
}
