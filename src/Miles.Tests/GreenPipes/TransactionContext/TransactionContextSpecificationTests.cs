using FakeItEasy;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using Miles.GreenPipes.TransactionContext;
using NUnit.Framework;
using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.TransactionContext
{
    [TestFixture]
    public class TransactionContextSpecificationTests
    {
        [TestCase(IsolationLevel.ReadCommitted)]
        [TestCase()]
        public void Given_Spec_When_Validate_Then_Validation(IsolationLevel? isolationLevel = null)
        {
            var sut = new TransactionContextSpecification<TestContext>();
            
            var result = sut.Validate();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [TestCase(IsolationLevel.ReadCommitted)]
        [TestCase()]
        public void Given_Spec_When_Apply_Then_FilterCreated(IsolationLevel? isolationLevel = null)
        {
            var pipeBuilder = A.Fake<IPipeBuilder<TestContext>>();

            var sut = new TransactionContextSpecification<TestContext>();
            var configurator = (ITransactionContextConfigurator)sut;

            configurator.HintIsolationLevel = isolationLevel;
            sut.Apply(pipeBuilder);

            A.CallTo(() => pipeBuilder.AddFilter(
                A<TransactionContextFilter<TestContext>>.That.NullCheckedMatches(
                    f => f.HintIsolationLevel == isolationLevel,
                    w => w.Write("Filter")))).MustHaveHappenedOnceExactly();
        }

        public interface TestContext : PipeContext
        { }
    }

}
