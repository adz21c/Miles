using FakeItEasy;
using GreenPipes;
using MassTransit;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Tests.MassTransit.MessageDeduplication
{
    [TestFixture]
    public class MessageDeduplicationFilterTests
    {
        private IServiceProvider _serviceProvider = A.Fake<IServiceProvider>();
        private readonly IConsumptionRecorder _consumptionRecorder = A.Fake<IConsumptionRecorder>();
        private readonly TestContext _context = A.Fake<TestContext>();
        private readonly IPipe<TestContext> _pipe = A.Fake<IPipe<TestContext>>();
        private const string QueueName = "queue-name";

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            A.CallTo(() => _context.TryGetPayload(out _serviceProvider)).Returns(true);
            A.CallTo(() => _serviceProvider.GetService(typeof(IConsumptionRecorder))).Returns(_consumptionRecorder);
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearRecordedCalls(_serviceProvider);
            Fake.ClearRecordedCalls(_consumptionRecorder);
            Fake.ClearRecordedCalls(_pipe);
            Fake.ClearRecordedCalls(_context);
        }

        [Test]
        public async Task Given_NewMessage_When_Send_Then_MessageRecordedAndPipeContinues()
        {
            A.CallTo(() => _consumptionRecorder.RecordAsync(A<MessageContext>._, A<string>._)).Returns(true);

            var sut = new MessageDeduplicationFilter<TestContext>(QueueName);
            await sut.Send(_context, _pipe);

            A.CallTo(() => _consumptionRecorder.RecordAsync(A<MessageContext>.That.IsNotNull(), QueueName)).MustHaveHappened()
                .Then(A.CallTo(() => _pipe.Send(_context)).MustHaveHappenedOnceExactly());
        }

        [Test]
        public async Task Given_OldMessage_When_Send_Then_MessageRecordAttemptAndNotContinuePipe()
        {
            A.CallTo(() => _consumptionRecorder.RecordAsync(A<MessageContext>._, A<string>._)).Returns(false);

            var sut = new MessageDeduplicationFilter<TestContext>(QueueName);
            await sut.Send(_context, _pipe);

            A.CallTo(() => _consumptionRecorder.RecordAsync(A<MessageContext>._, QueueName)).MustHaveHappened();
            A.CallTo(() => _pipe.Send(_context)).MustNotHaveHappened();
        }

        public interface TestContext : ConsumeContext
        { }
    }
}
