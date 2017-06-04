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
using FakeItEasy;
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.MessageDispatch;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Miles.UnitTests.MassTransit.TransactionContext
{
    [TestFixture]
    public class TransactionalMessagePublisherTests
    {
        [Test]
        public void Publish_DoesNotSaveMessages_WhenTransactionsAreNotCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext();
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatchProcess = A.Fake<IMessageDispatchProcess>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, fakeActivityContext, fakeMessageDispatchProcess);

            // Act
            ((IEventPublisher)publisher).Publish(eventType);
            ((ICommandPublisher)publisher).Publish(commandType);

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public async Task Publish_SaveMessages_WhenTransactionsAreCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext();
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatchProcess = A.Fake<IMessageDispatchProcess>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, fakeActivityContext, fakeMessageDispatchProcess);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync(new IsolationLevel?()))
            {
                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public async Task Publish_MessagesDispatched_WhenTransactionsAreCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext();
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatchProcess = A.Fake<IMessageDispatchProcess>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, fakeActivityContext, fakeMessageDispatchProcess);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync(new IsolationLevel?()))
            {
                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeMessageDispatchProcess.ExecuteAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public async Task Publish_MessagesAreNotDispatched_WhenTransactionsCommitFails()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext(failCommit: true);
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatchProcess = A.Fake<IMessageDispatchProcess>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, fakeActivityContext, fakeMessageDispatchProcess);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync(new IsolationLevel?()))
            {
                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeMessageDispatchProcess.ExecuteAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustNotHaveHappened();
        }

        //[Test]
        //public async Task Register_ProcessorsExecutedPriorToDatabaseCommit_WhenTransactionAreCommitted()
        //{
        //    // Arrange
        //    var eventType = new EventType();
        //    var commandType = new CommandType();

        //    var fakeTransactionContext = CreateTransactionContext();
        //    var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
        //    var fakeActivityContext = CreateActivityContext();
        //    var fakeMessageDispatchProcess = A.Fake<IMessageDispatchProcess>();

        //    var fakeEventProcessor = A.Fake<IMessageProcessor<EventType>>();
        //    var fakeCommandProcessor = A.Fake<IMessageProcessor<CommandType>>();

        //    var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, fakeActivityContext, fakeMessageDispatchProcess);

        //    // Act
        //    using (var transaction = await fakeTransactionContext.BeginAsync(new IsolationLevel?()))
        //    {
        //        ((IEventPublisher)publisher).Register(fakeEventProcessor);
        //        ((ICommandPublisher)publisher).Register(fakeCommandProcessor);

        //        ((IEventPublisher)publisher).Publish(eventType);
        //        ((ICommandPublisher)publisher).Publish(commandType);

        //        await transaction.CommitAsync();
        //    }

        //    // Assert
        //    A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustHaveHappened().Then(
        //        A.CallTo(() => fakeEventProcessor.ProcessAsync(A<EventType>.Ignored)).MustHaveHappened()).Then(
        //        A.CallTo(() => fakeMessageDispatchProcess.ExecuteAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened());

        //    A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustHaveHappened().Then(
        //        A.CallTo(() => fakeCommandProcessor.ProcessAsync(A<CommandType>.Ignored)).MustHaveHappened()).Then(
        //        A.CallTo(() => fakeMessageDispatchProcess.ExecuteAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened());
        //}

        private ITransactionContext CreateTransactionContext(bool failCommit = false)
        {
            var fakeTransaction = A.Fake<ITransaction>();
            var fakeTransactionContext = A.Fake<ITransactionContext>();
            var preCommitHook = new Hook<object, EventArgs>();
            var postCommitHook = new Hook<object, EventArgs>();

            A.CallTo(() => fakeTransaction.CommitAsync()).Invokes(async () =>
            {
                await preCommitHook.ExecuteAsync(fakeTransactionContext, new EventArgs());

                if (failCommit)
                    throw new Exception();

                await postCommitHook.ExecuteAsync(fakeTransactionContext, new EventArgs());
            }).Returns(Task.FromResult(0));

            A.CallTo(() => fakeTransactionContext.BeginAsync(new IsolationLevel?())).Returns(Task.FromResult(fakeTransaction));
            A.CallTo(() => fakeTransactionContext.PreCommit).Returns(preCommitHook);
            A.CallTo(() => fakeTransactionContext.PostCommit).Returns(postCommitHook);
            return fakeTransactionContext;
        }

        private IActivityContext CreateActivityContext()
        {
            var fakeActivityContext = A.Fake<IActivityContext>();
            A.CallTo(() => fakeActivityContext.CorrelationId).Returns(Guid.NewGuid());
            return fakeActivityContext;
        }
    }

    public class EventType
    { }

    public class CommandType
    { }
}
