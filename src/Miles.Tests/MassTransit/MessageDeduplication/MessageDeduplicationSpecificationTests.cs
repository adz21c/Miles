using FakeItEasy;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using Miles.GreenPipes.TransactionContext;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;
using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Miles.Tests.MassTransit.MessageDeduplication
{
    [TestFixture]
    public class MessageDeduplicationSpecificationTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_SpecWithoutQueueName_When_Validate_Then_ValidationError(string queueName)
        {
            var sut = new MessageDeduplicationSpecification<TestContext>();
            sut.QueueName = queueName;

            var result = sut.Validate();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            var queueError = result.FirstOrDefault();
            Assert.That(queueError, Is.Not.Null);
            Assert.That(queueError.Disposition, Is.EqualTo(ValidationResultDisposition.Failure));
            Assert.That(queueError.Message, Is.EqualTo(MessageDeduplicationSpecification<TestContext>.NoQueueNameError));
            Assert.That(queueError.Key, Is.EqualTo(MessageDeduplicationSpecification<TestContext>.QueueNameKey));
            Assert.That(queueError.Value, Is.EqualTo(queueName));
        }

        [Test]
        public void Given_Spec_When_Validate_Then_NoErrors()
        {
            const string QueueName = "QueueName";

            var sut = new MessageDeduplicationSpecification<TestContext>();
            sut.QueueName = QueueName;

            var result = sut.Validate();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }



        [Test]
        public void Given_Spec_Apply_Validate_Then_Filter()
        {
            const string QueueName = "QueueName";

            var pipeBuilder = A.Fake<IPipeBuilder<TestContext>>();

            var sut = new MessageDeduplicationSpecification<TestContext>();
            sut.QueueName = QueueName;

            sut.Apply(pipeBuilder);

            A.CallTo(() => pipeBuilder.AddFilter(
                A<MessageDeduplicationFilter<TestContext>>.That.NullCheckedMatches(
                    f => f.QueueName == QueueName,
                    w => w.Write("Filter")))).MustHaveHappenedOnceExactly();
        }

        public interface TestContext : ConsumeContext
        { }
    }

}
