using FakeItEasy;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using Miles.MassTransit.ActivityContext;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Tests.MassTransit.ActivityContext
{
    [TestFixture]
    public class ActivityContextExtensionsTests
    {
        [Test]
        public void Given_PipeConfigurator_When_UseActivityContext_Then_Specification()
        {
            var pipeConfigurator = A.Fake<IPipeConfigurator<ConsumeContext>>();

            pipeConfigurator.UseActivityContext();

            A.CallTo(() => pipeConfigurator.AddPipeSpecification(A<ActivityContextSpecification<ConsumeContext>>.That.IsNotNull())).MustHaveHappenedOnceExactly();
        }
    }

}
