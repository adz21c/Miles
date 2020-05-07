using FakeItEasy;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Tests.MassTransit.MessageDeduplication
{
    [TestFixture]
    public class MessageDeduplicationExtensionsTests
    {
        [Test]
        public void Given_PipeConfigurator_When_UseMessageDeduplication_Then_Specification()
        {
            var pipeConfigurator = A.Fake<IPipeConfigurator<ConsumeContext>>();
            const string QueueName = "QueueName";

            pipeConfigurator.UseMessageDeduplication(QueueName);

            A.CallTo(() => pipeConfigurator.AddPipeSpecification(
                A<MessageDeduplicationSpecification<ConsumeContext>>.That.NullCheckedMatches(
                    s => s.QueueName == QueueName,
                    w => w.Write("Filter")))).MustHaveHappenedOnceExactly();
        }
    }

}
