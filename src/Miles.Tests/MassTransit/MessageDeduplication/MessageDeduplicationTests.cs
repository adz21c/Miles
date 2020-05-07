using FakeItEasy;
using GreenPipes;
using MassTransit;
using MassTransit.TestFramework;
using MassTransit.Testing;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Tests.MassTransit.MessageDeduplication
{
    //[TestFixture]
    //class MessageDeduplicationTests
    //{
    //    private InMemoryTestHarness _testHarness;
    //    private ConsumerTestHarness<TestConsumer> _consumerHarness;
    //    private Guid _successId = Guid.NewGuid();
    //    private Guid _skipId = Guid.NewGuid();

    //    [OneTimeSetUp]
    //    public async Task Setup()
    //    {
    //        var consumptionRecorder = A.Fake<IConsumptionRecorder>();
    //        A.CallTo(() => consumptionRecorder.RecordAsync(
    //            A<MessageContext>.That.Matches(x => x.MessageId == _successId),
    //            A<string>._))
    //            .Returns(true);
    //        A.CallTo(() => consumptionRecorder.RecordAsync(
    //            A<MessageContext>.That.Matches(x => x.MessageId == _skipId),
    //            A<string>._))
    //            .Returns(false);

    //        var container = A.Fake<IContainer>();
    //        A.CallTo(() => container.Resolve<IConsumptionRecorder>(null)).Returns(consumptionRecorder);

    //        var containerScopeContext = A.Fake<ContainerScopeContext>();
    //        A.CallTo(() => containerScopeContext.Container).Returns(container);

    //        _testHarness = new InMemoryTestHarness();
    //        _testHarness.OnConfigureInMemoryReceiveEndpoint += c =>
    //        {
    //            c.UseInlineFilter((ctx, next) =>
    //            {
    //                ctx.GetOrAddPayload(() => containerScopeContext);
    //                return next.Send(ctx);
    //            });
    //            c.UseMessageDeduplication(_testHarness.InputQueueName);
    //        };
    //        _consumerHarness = _testHarness.Consumer<TestConsumer>();

    //        await _testHarness.Start();

    //        await _testHarness.InputQueueSendEndpoint.Send(new TestMessage(), s => s.MessageId = _successId);
    //        await _testHarness.InputQueueSendEndpoint.Send(new TestMessage(), s => s.MessageId = _skipId);
    //    }

    //    [OneTimeTearDown]
    //    public async Task TearDown()
    //    {
    //        await _testHarness.Stop();
    //    }

    //    [Test]
    //    public void MessagesSent()
    //    {
    //        var messageIds = _testHarness.Sent.Select<TestMessage>().Select(x => x.Context.MessageId).ToList();
    //        Assert.Contains(_successId, messageIds);
    //        Assert.Contains(_skipId, messageIds);
    //    }

    //    [Test]
    //    public void SuccessMessageConsumed()
    //    {
    //        Assert.Contains(_successId, _consumerHarness.Consumed.Select<TestMessage>().Select(x => x.Context.MessageId).ToList());
    //    }

    //    [Test]
    //    public void SkipMessageNotConsumed()
    //    {
    //        Assert.IsFalse(_consumerHarness.Consumed.Select<TestMessage>().Select(x => x.Context.MessageId).Contains(_skipId));
    //    }

    //    class TestMessage
    //    { }

    //    class TestConsumer : IConsumer<TestMessage>
    //    {
    //        public Task Consume(ConsumeContext<TestMessage> context)
    //        {
    //            return Task.CompletedTask;
    //        }
    //    }
    //}
}
