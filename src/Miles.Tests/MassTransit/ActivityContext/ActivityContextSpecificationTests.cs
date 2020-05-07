using FakeItEasy;
using GreenPipes;
using MassTransit;
using Miles.MassTransit.ActivityContext;
using Miles.MassTransit.MessageDeduplication;
using NUnit.Framework;

namespace Miles.Tests.MassTransit.ActivityContext
{
    [TestFixture]
    public class ActivityContextSpecificationTests
    {
        public void Given_Spec_When_Validate_Then_NoErrors()
        {
            var sut = new ActivityContextSpecification<ConsumeContext>();
     
            var result = sut.Validate();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Given_Spec_Apply_Validate_Then_Filter()
        {
            var pipeBuilder = A.Fake<IPipeBuilder<ConsumeContext>>();

            var sut = new ActivityContextSpecification<ConsumeContext>();
            
            sut.Apply(pipeBuilder);

            A.CallTo(() => pipeBuilder.AddFilter(A<ActivityContextFilter<ConsumeContext>>.That.IsNotNull())).MustHaveHappenedOnceExactly();
        }
    }

}
