using FakeItEasy;
using GreenPipes;
using MassTransit;
using Miles.MassTransit.ActivityContext;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.Tests.MassTransit.ActivityContext
{
    [TestFixture]
    public class ActivityContextFilterTests
    {
        private IServiceProvider _serviceProvider = A.Fake<IServiceProvider>();
        private readonly TestContext _context = A.Fake<TestContext>();
        private readonly IPipe<TestContext> _pipe = A.Fake<IPipe<TestContext>>();

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            A.CallTo(() => _context.TryGetPayload(out _serviceProvider)).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearRecordedCalls(_serviceProvider);
            Fake.ClearRecordedCalls(_pipe);
            Fake.ClearRecordedCalls(_context);
        }

        [TestCase("7a5c21cd-4f44-4344-baae-f36a41e70806", "b137e15d-001a-44a0-a85f-901867ca2eb8", "b137e15d-001a-44a0-a85f-901867ca2eb8")]
        public async Task Given_Filter_When_Send_Then_IdsSet(Guid? messageId, Guid? ctxCorrelationId, Guid correlationId)
        {
            A.CallTo(() => _context.MessageId).Returns(messageId);
            A.CallTo(() => _context.CorrelationId).Returns(ctxCorrelationId);
            
            var activityContext = new Miles.MassTransit.ActivityContext.ActivityContext();

            A.CallTo(() => _serviceProvider.GetService(typeof(Miles.MassTransit.ActivityContext.ActivityContext))).Returns(activityContext);

            var sut = new ActivityContextFilter<TestContext>();
            await sut.Send(_context, _pipe);

            Assert.That(activityContext.ActivityId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(activityContext.CorrelationId, Is.EqualTo(correlationId));
        }

        public interface TestContext : ConsumeContext
        { }
    }
}
